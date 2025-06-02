using Microsoft.EntityFrameworkCore;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.DataAccess.Repositories;

public class ArticlesInOrderRepository : IArticlesInOrderInterface<ArticlesInOrder>
{
    private readonly XmlCoreDbContext _context;

    public ArticlesInOrderRepository(XmlCoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ArticlesInOrder>> GetAllAsync()
    {
        return _context.ArticlesInOrders
            .Include(a => a.Order)
            .Include(o => o.Article)
            .ThenInclude(a => a.Category).ToList();
    }
    

   
}