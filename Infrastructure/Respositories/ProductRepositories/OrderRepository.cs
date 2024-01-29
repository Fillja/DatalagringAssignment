using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Respositories.ProductRepositories;

public class OrderRepository(SecondDataContext context) : BaseRepository<Order>(context)
{
    private readonly SecondDataContext _context = context;
}
