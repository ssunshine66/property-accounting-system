using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Impl;
using DAL.Repositories.Impl.Base;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DAL.Tests;

public class BaseRepositoryUnitTests
{
    [Fact]
    public void Update_InputUser_CalledUpdateMethodOfDbSet()
    {
        // Arrange
        DbContextOptions options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var mockContext = new Mock<ApplicationDbContext>(options);
        var mockSet = new Mock<DbSet<User>>();
        mockContext
            .Setup(context => context.Set<User>())
            .Returns(mockSet.Object);

        var repository = new BaseRepository<User>(mockContext.Object);
        User userToUpdate = new User() { Id = 1 };

        // Act
        repository.Update(userToUpdate);

        // Assert
        mockSet.Verify(set => set.Update(userToUpdate), Times.Once);
    }
    
    [Fact]
    public async Task Create_InputUserInstance_CalledAddMethodOfDBSetWithEmployeeInstance()
    {
        // Arrange
        DbContextOptions options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var mockContext = new Mock<ApplicationDbContext>(options);
        var mockSet = new Mock<DbSet<User>>();
        mockContext
            .Setup(context => context.Set<User>())
            .Returns(mockSet.Object);
        var repository = new UserRepository(mockContext.Object);
        User expectedUser = new User();

        // Act
        await repository.Create(expectedUser);

        // Assert
        mockSet.Verify(set => set.AddAsync(expectedUser, new CancellationToken()), Times.Once);
    }

    [Fact]
    public async Task Get_InputId_CalledFindMethodOfDBSetWithCorrectId()
    {
        // Arrange
        DbContextOptions options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var mockContext = new Mock<ApplicationDbContext>(options);
        var mockSet = new Mock<DbSet<User>>();
        mockContext
            .Setup(context => context.Set<User>())
            .Returns(mockSet.Object);
        var repository = new UserRepository(mockContext.Object);
        User expectedUser = new User()
        {
            Id = 1
        };
        mockSet.Setup(mock => mock.FindAsync(expectedUser.Id))
            .ReturnsAsync(expectedUser);

        // Act
        var actualUser = await repository.GetById(expectedUser.Id);

        // Assert
        mockSet.Verify(dbset => dbset.FindAsync(expectedUser.Id), Times.Once);
        Assert.Equal(expectedUser.Id, actualUser.Id);
    }

    [Fact]
    public async Task Delete_InputId_CalledRemoveMethodsOfDBSetWithCorrectArg()
    {
        // Arrange
        DbContextOptions options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var mockContext = new Mock<ApplicationDbContext>(options);
        var mockSet = new Mock<DbSet<User>>();
        
        mockContext
            .Setup(context => context.Set<User>())
            .Returns(mockSet.Object);
        
        var repository = new UserRepository(mockContext.Object);
        User expectedUser = new User()
        {
            Id = 1
        };
        mockSet.Setup(mock => mock.FindAsync(expectedUser.Id))
            .ReturnsAsync(expectedUser);

        // Act
        await repository.Delete(expectedUser.Id);

        // Assert
        mockSet.Verify(dbset => dbset.Remove(expectedUser), Times.Once);
    }
}