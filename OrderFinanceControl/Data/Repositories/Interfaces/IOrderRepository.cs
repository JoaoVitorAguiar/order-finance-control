using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Interfaces;

public interface IOrderRepository  
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(string id);
    Task<Order?> GetByIdWithDetailsAsync(string id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}
