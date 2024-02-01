using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class UserRepository_Tests
{
    private readonly DataContext _context = 
        new(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public void CreateShould_SaveOneEntityToDatabase_AndReturnTheEntity()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var entity = new UserEntity { Id = 1, FirstName = "Test", LastName = "Testsson" };

        //Act
        var result = _userRepository.Create(entity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id); 
        Assert.Equal("Test", result.FirstName);
        Assert.IsType<UserEntity>(result);
    }

    [Fact]
    public void CreateShould_NotAddToDatabase_AndReturnNull()
    {
        //Arrange
        var _userRepository = new UserRepository(_context);
        var entity = new UserEntity { FirstName = "Test" };

        //Act
        var result = _userRepository.Create(entity);

        //Assert
        Assert.Null(result);
    }



}
