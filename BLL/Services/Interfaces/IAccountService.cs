using BLL.DTOs;
using CCL.Security.Identity;

namespace BLL.Services.Interfaces;

public interface IAccountService
{
    Task CreateUser(CreateUserDto userDto);
    Task DeleteUser(int id);
    Task AssignRole(int userId, Role role);
    IEnumerable<CreateUserDto> GetUsersFiltered(int pageNumber);
}