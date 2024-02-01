using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class AddressRepository_Tests
{
    private readonly DataContext _context =
    new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void GetAllShould_GetAllAddressEntities_AndReturnIEnumerableOfTypeAddressEntity()
    {
        //Arrange
        var _adressRepository = new AddressRepository(_context);
        var addressEntity = new AddressEntity { Id = 1, City = "Karlshamn", Street = "Bodekullsvägen 11b", PostalCode = "37435" };
        _adressRepository.Create(addressEntity);

        //Act
        var result = _adressRepository.GetAll();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<AddressEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public void GetAllShould_GetNoAddressEntities_AndReturnEmptyIEnumerableOfTypeAddressEntity()
    {
        //Arrange
        var _adressRepository = new AddressRepository(_context);

        //Act
        var result = _adressRepository.GetAll();

        //Assert
        Assert.Empty(result);
        Assert.IsAssignableFrom<IEnumerable<AddressEntity>>(result);
    }
}
