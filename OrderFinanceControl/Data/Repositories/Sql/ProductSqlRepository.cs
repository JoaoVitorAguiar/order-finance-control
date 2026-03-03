using Microsoft.EntityFrameworkCore;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Sql;

public class ProductSqlRepository(OrderFinanceControlDbContext context) : IProductRepository
{
    private readonly OrderFinanceControlDbContext _context = context;

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context
            .Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _context.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<Product?> GetByNameAsync(string name)
    {
        return await _context
            .Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name);
    }
}