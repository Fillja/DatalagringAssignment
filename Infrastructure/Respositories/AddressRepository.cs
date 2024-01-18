using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Respositories;

public class AddressRepository(DataContext context) : BaseRepository<AddressEntity>(context)
{
    private readonly DataContext _context = context;
}
