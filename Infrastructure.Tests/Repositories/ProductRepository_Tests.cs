using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Respositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class ProductRepository_Tests
{
    private readonly SecondDataContext _context =
    new(new DbContextOptionsBuilder<SecondDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void UpdateShould_CheckIfProductEntityExists_ThenUpdateTheEntity_AndReturnTheEntity()
    {
        //Arrange
        var _productRepository = new ProductRepository(_context);
        var productEntity = new Product { Id = 1, CategoryId = 1, ManufacturerId = 1, Title = "Test" ,Description = "Test", Price = 100 };
        _productRepository.Create(productEntity);

        //Act
        productEntity.Title = "Updated Test";
        var result = _productRepository.Update(productEntity, x => x.Id == productEntity.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Test", result.Title);
    }

    [Fact]
    public void UpdateShould_CheckIfProductEntityExists_ThenNotFindEntity_AndReturnNull()
    {
        //Arrange
        var _productRepository = new ProductRepository(_context);
        var productEntity = new Product { Id = 1, CategoryId = 1, ManufacturerId = 1, Title = "Test", Description = "Test", Price = 100 };

        //Act
        var result = _productRepository.Update(productEntity, x => x.Id == productEntity.Id);

        //Assert
        Assert.Null(result);
    }
}
