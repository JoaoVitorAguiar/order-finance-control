using OrderFinanceControl.Common;
using OrderFinanceControl.Common.Errors;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Dtos.Orders;

namespace OrderFinanceControl.UseCases.Orders;

public class GetOrderByIdUseCase(IOrderRepository orderRepository)
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<OrderResponseDto>> ExecuteAsync(int id)
    {
        var order = await _orderRepository.GetByIdWithDetailsAsync(id);

        if (order is null)
            return OrderErrors.NotFound;

        return new OrderResponseDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Status = order.Status.ToString(),

            Customer = new CustomerResponseDto
            {
                Id = order.Customer.Id,
                Name = order.Customer.Name
            },

            Items = order.Items.Select(i => new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPriceAtOrderTime = i.UnitPriceAtOrderTime
            }).ToList()
        };
    }
}
