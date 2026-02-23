using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids);
    Task<Product?> GetByNameAsync(string name);
    Task AddAsync(Product product);
    Task SaveChangesAsync();
}