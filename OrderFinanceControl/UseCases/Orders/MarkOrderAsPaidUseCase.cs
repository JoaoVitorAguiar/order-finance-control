using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Enums;
using OrderFinanceControl.Exceptions;

namespace OrderFinanceControl.UseCases.Orders;

public class MarkOrderAsPaidUseCase
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsPaidUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ExecuteAsync(int orderId)
    {
        var order = await _orderRepository.GetEntityByIdAsync(orderId);

        if (order == null)
            throw new NotFoundException("Order not found.");

        if (order.Status != OrderStatus.Created)
            throw new ConflictException("Order cannot be paid.");

        order.Status = OrderStatus.Paid;
        order.PaidAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);
    }
}