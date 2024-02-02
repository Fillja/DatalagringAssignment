using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Factories;
using Infrastructure.Respositories.ProductRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Services;

public class ProductService_Tests
{
    private readonly SecondDataContext _context =
    new(new DbContextOptionsBuilder<SecondDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public void CreateProductShould_TakeProductDtoAsParam_ThenCreateAndSaveAllEntities_AndReturnTrue()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);
        var _productService = new ProductService(_productFactories, _productRepository);

        var product = new ProductDto
        {
            Id = 1,
            Title = "TestTitle",
            Description = "TestDescription",
            Price = 100,
            CategoryName = "TestCategory",
            ManufacturerName = "TestManufacturer",
        };

        //Act
        var result = _productService.CreateProduct(product);

        //Arrange
        Assert.True(result);
    }

    [Fact]
    public void CreateProductShould_TakeIncompleteProductDtoAsParam_ThenFailToCreateAllEntities_AndReturnFalse()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);
        var _productService = new ProductService(_productFactories, _productRepository);

        var product = new ProductDto();

        //Act
        var result = _productService.CreateProduct(product);

        //Arrange
        Assert.False(result);
    }

    [Fact]
    public void GetAllProductsShould_GetAllProductEntities_ThenCompileAllEntitiesIntoProductDto_ThenAddCompleteProductDtoToNewList_AndReturnListAsIEnumerableOfTypeProductDto()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);
        var _productService = new ProductService(_productFactories, _productRepository);

        var categoryEntity = new Category { Id = 1, CategoryName = "TestCategory" };
        var manufacturerEntity = new Manufacturer { Id = 1, ManufacturerName = "TestManufacturer" };
        var productEntity = new Product 
        {  
            Id = 1, 
            Title = "TestTitle", 
            Description = "TestDescription",
            Price = 1,
            CategoryId = 1,
            ManufacturerId = 1,
            Category = categoryEntity,
            Manufacturer = manufacturerEntity
        };

        //Act
        _categoryRepository.Create(categoryEntity);
        _manufacturerRepository.Create(manufacturerEntity);
        _productRepository.Create(productEntity);

        var result = _productService.GetAllProducts();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProductDto>>(result);
        Assert.Single(result);
    }

    [Fact]
    public void GetAllProductsShould_GetNoProductEntities_ThenCompileNothing_ThenAddEmptyProductDtoToNewList_AndReturnEmptyListAsIEnumerableOfTypeProductDto()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);
        var _productService = new ProductService(_productFactories, _productRepository);

        //Act
        var result = _productService.GetAllProducts();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProductDto>>(result);
        Assert.Empty(result);
    }
}
