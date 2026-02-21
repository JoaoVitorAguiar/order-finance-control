using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByNameAsync(string name);
    Task AddAsync(Product product);
    Task SaveChangesAsync();
}