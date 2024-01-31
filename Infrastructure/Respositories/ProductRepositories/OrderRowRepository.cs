using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Respositories.ProductRepositories;

public class OrderRowRepository(SecondDataContext context) : BaseRepository<OrderRow>(context)
{
    private readonly SecondDataContext _context = context;

    public override IEnumerable<OrderRow> GetAll()
    {
        var entity = base.GetAll();
        entity = _context.OrderRows.Include(i => i.Order).Include(i => i.Product).ToList();
        return entity;
    }
}

