using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Respositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class ManufacturerRepository_Tests
{
    private readonly SecondDataContext _context =
    new(new DbContextOptionsBuilder<SecondDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void GetAllShould_GetAllManufacturerEntities_AndReturnIEnumerableOfTypeManufacturerEntity()
    {
        //Arrange
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var manufacturerEntity = new Manufacturer { Id = 1, ManufacturerName = "Test" };
        _manufacturerRepository.Create(manufacturerEntity);

        //Act
        var result = _manufacturerRepository.GetAll();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Manufacturer>>(result);
        Assert.Single(result);
    }

    [Fact]
    public void GetAllShould_GetNoManufacturerEntities_AndReturnEmptyIEnumerableOfTypeManufacturerEntity()
    {
        //Arrange
        var _manufacturerRepository = new ManufacturerRepository(_context);

        //Act
        var result = _manufacturerRepository.GetAll();

        //Assert
        Assert.Empty(result);
        Assert.IsAssignableFrom<IEnumerable<Manufacturer>>(result);
    }
}
