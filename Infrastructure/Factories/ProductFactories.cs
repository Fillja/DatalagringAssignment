using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Respositories;
using Infrastructure.Respositories.ProductRepositories;
using System.Diagnostics;

namespace Infrastructure.Factories;

public class ProductFactories(CategoryRepository categoryRepository, ManufacturerRepository manufacturerRepository, ProductRepository productRepository, OrderRepository orderRepository, OrderRowRepository orderRowRepository)
{
    private readonly CategoryRepository _categoryRepository = categoryRepository;
    private readonly ManufacturerRepository _manufacturerRepository = manufacturerRepository;
    private readonly ProductRepository _productRepository = productRepository;
    private readonly OrderRepository _orderRepository = orderRepository;
    private readonly OrderRowRepository _orderRowRepository = orderRowRepository;

    //public ProductDto CompileFullProduct(Product entity)
    //{
    //    try
    //    {
    //        var categoryEntity = _categoryRepository.GetOne(x => x.Id == entity.CategoryId);
    //        var manufacturerEntity = _manufacturerRepository.GetOne(x => x.Id == entity.ManufacturerId);

    //        var productDto = new ProductDto 
    //        {
    //            Title = entity.Title,
    //            Description = entity.Description,
    //            Price = entity.Price,
    //            CategoryName = categoryEntity.CategoryName,
    //            ManufacturerName = manufacturerEntity.ManufacturerName
    //        };
    //        return productDto;
    //    }
    //    catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
    //    return null!;
    //}

    public Category GetOrCreateCategoryEntity(string categoryName)
    {
        try
        {
            if (_categoryRepository.Exists(x => x.CategoryName == categoryName))
            {
                var categoryEntity = _categoryRepository.GetOne(x => x.CategoryName == categoryName);
                return categoryEntity;
            }
            else
            {
                var categoryEntity = new Category
                {
                    CategoryName = categoryName,
                };

                categoryEntity = _categoryRepository.Create(categoryEntity);
                return categoryEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }

    public Manufacturer GetOrCreateManufacturerEntity(string manufacturerName)
    {
        try
        {
            if (_manufacturerRepository.Exists(x => x.ManufacturerName == manufacturerName))
            {
                var manufacturerEntity = _manufacturerRepository.GetOne(x => x.ManufacturerName == manufacturerName);
                return manufacturerEntity;
            }
            else
            {
                var manufacturerEntity = new Manufacturer
                {
                    ManufacturerName = manufacturerName,
                };

                manufacturerEntity = _manufacturerRepository.Create(manufacturerEntity);
                return manufacturerEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }

    public Product CreateProductEntity(string title, string description, decimal price, int categoryId, int manufacturerId)
    {
        try
        {
            var productEntity = new Product
            {
                Title = title,
                Description = description,
                Price = price, 
                CategoryId = categoryId,
                ManufacturerId = manufacturerId
            };

            productEntity = _productRepository.Create(productEntity);
            return productEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }

    public Order CreateOrderEntity(int userId)
    {
        try
        {
            var orderEntity = new Order { UserId = userId };

            orderEntity = _orderRepository.Create(orderEntity);
            return orderEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }

    public OrderRow CreateOrderRowEntity(int orderId, int productId, int amount)
    {
        try
        {
            var orderRowEntity = new OrderRow 
            {
                OrderId = orderId,
                ProductId = productId,
                Amount = amount
            };

            orderRowEntity = _orderRowRepository.Create(orderRowEntity);
            return orderRowEntity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return null!;
    }
}
