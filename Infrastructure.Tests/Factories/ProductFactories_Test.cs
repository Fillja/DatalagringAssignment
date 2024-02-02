using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Factories;
using Infrastructure.Respositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Factories;

public class ProductFactories_Test
{
    private readonly SecondDataContext _context =
    new(new DbContextOptionsBuilder<SecondDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);


    [Fact]
    public void CompileFullProductShould_TakeProductEntityAsParam_AndBuildCompleteProductDto_ThenReturnProductDto()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);

        var categoryEntity = new Category { Id = 1, CategoryName = "Test"};
        var manufacturerEntity = new Manufacturer { Id = 1, ManufacturerName = "Test" };
        var productEntity = new Product 
        { 
            Id = 1, Title = "Test", 
            Description = "Test", 
            Price = 100, 
            CategoryId = 1, 
            ManufacturerId = 1, 
            Category = categoryEntity, 
            Manufacturer = manufacturerEntity 
        };

        //Act
        _categoryRepository.Create(categoryEntity);
        _manufacturerRepository.Create(manufacturerEntity);

        var result = _productFactories.CompileFullProduct(productEntity);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ProductDto>(result);
        Assert.Equal("Test", result.Title);
        Assert.Equal("Test", result.CategoryName);
        Assert.Equal("Test", result.ManufacturerName);
    }

    [Fact]
    public void CompileFullProductShould_TakeIncompleteProductEntityAsParam_AndFailToBuildCompleteProductDto_ThenReturnNull()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);


        var productEntity = new Product
        {
            Id = 1,
            Title = "Test",
            Description = "Test",
            Price = 100,
            CategoryId = 1,
            ManufacturerId = 1,
        };

        //Act
        var result = _productFactories.CompileFullProduct(productEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetOrCreateCategoryEntityShould_TakePropertyAsParam_ThenCheckIfParamExist_ThenCreateTheEntity_AndReturnTheEntity()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);

        var categoryName = "TestCategory";

        //Act
        var result = _productFactories.GetOrCreateCategoryEntity(categoryName);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Category>(result);
        Assert.Equal(categoryName, result.CategoryName);
    }

    [Fact]
    public void GetOrCreateCategoryEntityShould_TakePropertyAsParam_ThenCheckIfParamExist_ThenFindExistingEntity_AndReturnTheEntity()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);

        var categoryName = "TestCategory";

        var categoryEntity = new Category { Id = 1, CategoryName = categoryName };

        //Act
        _categoryRepository.Create(categoryEntity);

        var result = _productFactories.GetOrCreateCategoryEntity(categoryName);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Category>(result);
        Assert.Equal(categoryName, result.CategoryName);
    }

    [Fact]
    public void GetOrCreateManufacturerEntityShould_TakePropertyAsParam_ThenCheckIfParamExist_ThenCreateTheEntity_AndReturnTheEntity()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);

        var manufacturerName = "TestManufacturer";

        //Act
        var result = _productFactories.GetOrCreateManufacturerEntity(manufacturerName);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Manufacturer>(result);
        Assert.Equal(manufacturerName, result.ManufacturerName);
    }

    [Fact]
    public void GetOrCreateManufacturerEntityShould_TakePropertyAsParam_ThenCheckIfParamExist_ThenFindExistingEntity_AndReturnTheEntity()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);

        var manufacturerName = "TestManufacturer";

        var manufacturerEntity = new Manufacturer { Id = 1, ManufacturerName = manufacturerName };

        //Act
        _manufacturerRepository.Create(manufacturerEntity);

        var result = _productFactories.GetOrCreateManufacturerEntity(manufacturerName);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Manufacturer>(result);
        Assert.Equal(manufacturerName, result.ManufacturerName);
    }

    [Fact]
    public void CreateProductEntityShould_TakePropertiesAsParams_ThenCreateProductEntity_AndReturnTheEntity()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);

        var title = "TestTitle";
        var description = "TestDescription";
        var price = 100;
        var categoryId = 1;
        var manufacturerId = 1;

        //Act
        var result = _productFactories.CreateProductEntity(title, description, price, categoryId, manufacturerId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Product>(result);
        Assert.Equal(title, result.Title);
    }

    [Fact]
    public void CreateOrderEntityShould_TakePropertyAsParam_ThenCreateOrderEntity_AndReturnTheEntity()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);

        var userId = 1;

        //Act
        var result = _productFactories.CreateOrderEntity(userId);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<Order>(result);
        Assert.Equal(userId, result.UserId);
    }

    [Fact]
    public void CreateOrderRowEntityShould_TakePropertiesAsParams_ThenCreateOrderRowEntity_AndReturnTheEntity()
    {
        //Arrange
        var _categoryRepository = new CategoryRepository(_context);
        var _manufacturerRepository = new ManufacturerRepository(_context);
        var _productRepository = new ProductRepository(_context);
        var _orderRepository = new OrderRepository(_context);
        var _orderRowRepository = new OrderRowRepository(_context);
        var _productFactories = new ProductFactories(_categoryRepository, _manufacturerRepository, _productRepository, _orderRepository, _orderRowRepository);

        var orderId = 1;
        var productId = 1;
        var amount = 1;

        //Act
        var result = _productFactories.CreateOrderRowEntity(orderId, productId, amount);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OrderRow>(result);
        Assert.Equal(orderId, result.OrderId);
    }
}
