using AutoMapper;
using BLL.DTOs;
using BLL.Services.Interfaces;
using CCL.Security;
using CCL.Security.Identity;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using User = DAL.Entities.User;

namespace BLL.Services.Impl;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private int pageSize = 10;
    
    public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        ArgumentNullException.ThrowIfNull(unitOfWork);
        _userRepository = unitOfWork.Users;
        _mapper = mapper;
    }
    
    // <exception cref="MethodAccessException"></exception>
    public IEnumerable<CreateUserDto> GetUsersFiltered(int pageNumber)
    {
        var user = SecurityContext.GetUser();
        if (user == null || !user.Roles.Contains(Role.Administrator))
        {
            throw new MethodAccessException();
        }

        var userId = user.UserId;
        var users = _unitOfWork.Users.Find(e => e.Id == userId, pageNumber, pageSize);

        var usersDto = _mapper.Map<List<CreateUserDto>>(users);
        return usersDto;
    }

    public async Task CreateUser(CreateUserDto userDto)
    {
        var userExists = await _userRepository.GetByNameAsync(userDto.Name);
        if (userExists != null)
        {
            throw new ApplicationException("User already exists");
        }
        
        var userEntity = _mapper.Map<User>(userDto);
        
        await _userRepository.Create(userEntity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var userExists = await _userRepository.GetById(id);
        if (userExists == null)
        {
            throw new ApplicationException("User not found");
        }
        
        await _userRepository.Delete(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignRole(int userId, Role role)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            throw new ApplicationException("User not found");
        }
        
        user.Role = role;
        
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }
}