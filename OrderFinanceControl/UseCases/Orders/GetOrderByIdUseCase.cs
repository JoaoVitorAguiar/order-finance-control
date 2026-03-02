using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Orders;

namespace OrderFinanceControl.UseCases.Orders;

public class GetOrderByIdUseCase(IOrderRepository orderRepository)
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<OrderResponseDto?> ExecuteAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }
}
