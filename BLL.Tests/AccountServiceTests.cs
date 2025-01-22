using AutoMapper;
using BLL.DTOs;
using BLL.Services.Impl;
using BLL.Tests.Fake;
using CCL.Security;
using CCL.Security.Identity;
using DAL.UnitOfWork;
using Moq;

namespace BLL.Tests;

public class AccountServiceTests
{
    [Fact]
    public void Ctor_InputNull_ThrowArgumentNullException()
    {
        // Arrange
        IUnitOfWork nullUnitOfWork = null;
        var mockedMapper = new Mock<IMapper>();
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => new AccountService(nullUnitOfWork, mockedMapper.Object));
    }

    [Fact]
    public void GetEmployees_EmployeeFromDAL_CorrectMappingToOrderDTO()
    {
        // Arrange
        User user = new User(1, new List<Role>{ Role.Administrator });
        SecurityContext.SetUser(user);

        var expectedUserDto = new CreateUserDto()
        {
            Name = "Name",
            Email = "ukr@ukr.net",
            Password = "password"
        };
        
        var accountServiceFake = new AccountServiceFake(expectedUserDto);
        var actualService = accountServiceFake.Get();

        // Act
        var actualEmployeeDto = actualService.GetUsersFiltered(0).First();

        // Assert
        Assert.True(actualEmployeeDto.Name == expectedUserDto.Name && actualEmployeeDto.Email == expectedUserDto.Email && actualEmployeeDto.Password == expectedUserDto.Password);
    }
}