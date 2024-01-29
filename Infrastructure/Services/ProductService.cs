using Infrastructure.Dtos;
using Infrastructure.Factories;
using Infrastructure.Respositories.ProductRepositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ProductService(ProductFactories productFactories)
{
    private readonly ProductFactories _productFactories = productFactories;

    public bool CreateProduct(ProductDto product)
    {
        try
        {
            var categoryEntity = _productFactories.GetOrCreateCategoryEntity(product.CategoryName);

            var manufacturerEntity = _productFactories.GetOrCreateManufacturerEntity(product.ManufacturerName);

            var productEntity = _productFactories.CreateProductEntity(product.Title, product.Description, product.Price, categoryEntity.Id, manufacturerEntity.Id);

            if (productEntity != null)
                return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR:: " + ex.Message); }
        return false;
    }
}
