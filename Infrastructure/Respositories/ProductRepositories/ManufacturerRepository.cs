using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Respositories.ProductRepositories;

public class ManufacturerRepository(SecondDataContext context) : BaseRepository<Manufacturer>(context)
{
    private readonly SecondDataContext _context = context;
}
