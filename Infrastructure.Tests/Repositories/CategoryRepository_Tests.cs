using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Respositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class CategoryRepository_Tests
{
    private readonly SecondDataContext _context =
    new(new DbContextOptionsBuilder<SecondDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void CreateShould_SaveOneCategoryEntityToDatabase_AndReturnTheEntity()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var entity = new Category { Id = 1, CategoryName = "Test" };

        //Act
        var result = _categoryRepository.Create(entity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.CategoryName);
        Assert.IsType<Category>(result);
    }

    [Fact]
    public void CreateShould_NotAddToDatabase_AndReturnNull()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var entity = new Category { Id = 1 };

        //Act
        var result = _categoryRepository.Create(entity);

        //Assert
        Assert.Null(result);
    }
}
