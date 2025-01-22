using AutoMapper;
using BLL.DTOs;
using BLL.Services.Impl;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Moq;

namespace BLL.Tests.Fake;

public class AccountServiceFake
{
    private CreateUserDto userDto;

    public AccountServiceFake(CreateUserDto userDto)
    {
        this.userDto = userDto;
    }

    public IAccountService Get()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockMapper = new Mock<IMapper>();

        var expectedUser = new User()
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = userDto.Password
        };

        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(repo => repo.Find(It.IsAny<Func<User, bool>>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(new List<User> { expectedUser });

        mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepository.Object);

        mockMapper
            .Setup(m => m.Map<List<CreateUserDto>>(It.IsAny<List<User>>()))
            .Returns(new List<CreateUserDto> { userDto });

        IAccountService accountService = new AccountService(mockUnitOfWork.Object, mockMapper.Object);
        return accountService;
    }
}