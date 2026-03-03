using Microsoft.EntityFrameworkCore;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Sql;

public class OrderSqlRepository(OrderFinanceControlDbContext dbContext) : IOrderRepository
{
    private readonly OrderFinanceControlDbContext _dbContext = dbContext;
    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _dbContext.Orders
        .Include(o => o.Customer)
        .Include(o => o.Items)
            .ThenInclude(i => i.Product)
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<Order?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }
    public async Task UpdateAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
    }

}
