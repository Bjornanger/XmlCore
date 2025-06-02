using Microsoft.EntityFrameworkCore;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.DataAccess.Repositories;

public class OrderRepository : IOrderInterface<Order>
{
    private readonly XmlCoreDbContext _context;

    public OrderRepository(XmlCoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return _context.Orders
            .Include(o => o.Company)
            .Include(a => a.ArticlesInOrderList)
            .ThenInclude(ao => ao.Article)
            .ThenInclude(a => a.Category).ToList();
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task CreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        _context.SaveChangesAsync();
    }

    public Task<Order> UpdateAsync(Order entity, int id)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        var orderToRemove = await _context.Orders.FindAsync(id);
        if (orderToRemove is null)
        {
            return;
        }

        _context.Remove(orderToRemove);
        _context.SaveChangesAsync();
    }

    public async Task<Order> GetOrderByOrderNumberAsync(int orderNumber)
    {
        return await _context.Orders.Include(o => o.Company)
            .Include(o => o.ArticlesInOrderList)
            .ThenInclude(o => o.Article)
            .ThenInclude(a => a.Category)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }


}