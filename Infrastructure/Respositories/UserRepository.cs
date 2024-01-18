using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Respositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity>(context)
{
    private readonly DataContext _context = context;
}
