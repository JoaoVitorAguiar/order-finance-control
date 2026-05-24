using OrderFinanceControl.Common;
using OrderFinanceControl.Common.Errors;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Enums;

namespace OrderFinanceControl.UseCases.Orders;

public class MarkOrderAsPaidUseCase
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsPaidUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<bool>> ExecuteAsync(string orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
            return OrderErrors.NotFound;

        if (order.Status != OrderStatus.Created)
            return OrderErrors.CannotBePaid;

        order.Status = OrderStatus.Paid;
        order.PaidAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);

        return true;
    }
}
