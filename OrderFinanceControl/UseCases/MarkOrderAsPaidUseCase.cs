using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Enums;
using OrderFinanceControl.Exceptions;

namespace OrderFinanceControl.UseCases;

public class MarkOrderAsPaidUseCase
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsPaidUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ExecuteAsync(string orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
            throw new NotFoundException("Order not found.");

        if (order.Status != OrderStatus.Created)
            throw new ConflictException("Order cannot be paid.");

        order.Status = OrderStatus.Paid;
        order.PaidAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);
    }
}