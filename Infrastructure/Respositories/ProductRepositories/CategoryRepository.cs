using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Respositories.ProductRepositories;

public class CategoryRepository(SecondDataContext context) : BaseRepository<Category>(context)
{
    private readonly SecondDataContext _context = context;
}
