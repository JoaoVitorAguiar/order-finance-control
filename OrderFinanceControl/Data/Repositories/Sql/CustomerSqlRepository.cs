using Microsoft.EntityFrameworkCore;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Sql;

public class CustomerSqlRepository(OrderFinanceControlDbContext context) : ICustomerRepository
{
    private readonly OrderFinanceControlDbContext _context = context;

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context
            .Customers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context
            .Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context
            .Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}