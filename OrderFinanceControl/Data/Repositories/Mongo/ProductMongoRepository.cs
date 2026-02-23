using MongoDB.Driver;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Mongo;

public class ProductMongoRepository(MongoContext mongoContext) : IProductRepository
{
    private readonly MongoContext _mongoContext = mongoContext;

    public async Task AddAsync(Product product)
    {
        await _mongoContext.Products.InsertOneAsync(product);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _mongoContext.Products
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<string> ids)
    {
        return await _mongoContext.Products
            .Find(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<Product?> GetByNameAsync(string name)
    {
        return await _mongoContext.Products
            .Find(p => p.Name == name)
            .FirstOrDefaultAsync();
    }
}