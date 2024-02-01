using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class ProfileRepository_Tests
{
    private readonly DataContext _context =
    new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void ExistsShould_CheckIfProfileEntityExists_AndReturnTrue()
    {
        //Arrange
        var _profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity { AddressId = 1, RoleId = 1, UserId = 1 };
        _profileRepository.Create(profileEntity);

        //Act
        var result = _profileRepository.Exists(x => x.UserId == profileEntity.UserId);

        //Assert
        Assert.True(result);
        Assert.Equal(1, profileEntity.AddressId);
    }

    [Fact]
    public void ExistsShould_CheckIfProfileEntityExists_AndReturnFalse()
    {
        //Arrange
        var _profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity { AddressId = 1, RoleId = 1, UserId = 1 };

        //Act
        var result = _profileRepository.Exists(x => x.UserId == profileEntity.UserId);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void DeleteShould_CheckIfProfileEntityIsNull_ThenDeleteTheEntity_AndReturnTrue()
    {
        //Arrange
        var _profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity { AddressId = 1, RoleId = 1, UserId = 1 };
        _profileRepository.Create(profileEntity);

        //Act
        var result = _profileRepository.Delete(x => x.UserId == profileEntity.UserId);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void DeleteShould_CheckIfProfileEntityIsNull_AndReturnFalse()
    {
        //Arrange
        var _profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity();

        //Act
        var result = _profileRepository.Delete(x => x.UserId == profileEntity.UserId);

        //Assert
        Assert.False(result);
    }
}
