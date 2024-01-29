using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Respositories.ProductRepositories;

public class OrderRowRepository(SecondDataContext context) : BaseRepository<OrderRow>(context)
{
    private readonly SecondDataContext _context = context;
}

