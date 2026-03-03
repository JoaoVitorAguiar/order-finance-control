using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order?> GetByIdWithDetailsAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}
