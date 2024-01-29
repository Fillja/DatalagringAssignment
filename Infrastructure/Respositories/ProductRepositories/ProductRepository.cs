using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Respositories.ProductRepositories;

public class ProductRepository(SecondDataContext context) : BaseRepository<Product>(context)
{
    private readonly SecondDataContext _context = context;
}
