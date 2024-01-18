using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Respositories;

public class VerificationRepository(DataContext context) : BaseRepository<VerificationEntity>(context)
{
    private readonly DataContext _context = context;
}
