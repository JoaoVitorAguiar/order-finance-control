using MongoDB.Driver;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Mongo;

public class OrderMongoRepository(MongoContext mongoContext) : IOrderRepository
{
    private readonly MongoContext _mongoContext = mongoContext;

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _mongoContext.Orders
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(string id)
    {
        return await _mongoContext.Orders
            .Find(o => o.Id == id)
            .FirstOrDefaultAsync();
    }

    public Task<Order?> GetByIdWithDetailsAsync(string id)
    {
        return GetByIdAsync(id);
    }

    public async Task AddAsync(Order order)
    {
        await _mongoContext.Orders.InsertOneAsync(order);
    }

    public async Task UpdateAsync(Order order)
    {
        await _mongoContext.Orders.ReplaceOneAsync(
                o => o.Id == order.Id,
                order
            );
    }
}
