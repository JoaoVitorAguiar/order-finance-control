using MongoDB.Driver;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Mongo;

public class CustomerMongoRepository(MongoContext mongoContext) : ICustomerRepository
{
    private readonly MongoContext _mongoContext = mongoContext;

    public async Task AddAsync(Customer customer)
    {
        await _mongoContext.Customers.InsertOneAsync(customer);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _mongoContext.Customers
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _mongoContext.Customers
            .Find(c => c.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<Customer?> GetByIdAsync(string id)
    {
        return await _mongoContext.Customers
            .Find(c => c.Id == id)
            .FirstOrDefaultAsync();
    }
}
