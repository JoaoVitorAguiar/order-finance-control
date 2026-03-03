using OrderFinanceControl.Dtos.Orders;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto?> GetByIdAsync(int id);
    Task<Order?> GetEntityByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}
