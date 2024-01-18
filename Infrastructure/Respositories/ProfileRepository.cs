using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Respositories;

public class ProfileRepository(DataContext context) : BaseRepository<ProfileEntity>(context)
{
    private readonly DataContext _context = context;
}
