using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Orders;

namespace OrderFinanceControl.UseCases.Orders;

public class GetOrdersUseCase(IOrderRepository orderRepository)
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<IEnumerable<OrderResponseDto>> ExecuteAsync()
    {
        return await _orderRepository.GetAllAsync();
    }
}
