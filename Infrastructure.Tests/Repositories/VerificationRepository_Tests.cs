using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class VerificationRepository_Tests
{
    private readonly DataContext _context =
    new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void GetOneShould_FindOneVerificationEntity_AndReturnTheEntity()
    {
        //Arrange
        var _verificationRepository = new VerificationRepository(_context);
        var verificationEntity = new VerificationEntity { UserId = 1, Email = "Test@domain.com", Password = "Test"};
        _verificationRepository.Create(verificationEntity);

        //Act
        var result = _verificationRepository.GetOne(x => x.Email == verificationEntity.Email);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Test@domain.com", result.Email);
    }

    [Fact]
    public void GetOneShould_NotFindOneVerificationEntity_AndReturnNull()
    {
        //Arrange
        var _verificationRepository = new VerificationRepository(_context);
        var verificationEntity = new VerificationEntity { UserId = 1, Email = "Test@domain.com", Password = "Test" };

        //Act
        var result = _verificationRepository.GetOne(x => x.Email == verificationEntity.Email);

        //Assert
        Assert.Null(result);
    }
}
