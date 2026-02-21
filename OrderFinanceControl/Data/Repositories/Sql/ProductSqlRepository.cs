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
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context
            .Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetByNameAsync(string name)
    {
        return await _context
            .Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}