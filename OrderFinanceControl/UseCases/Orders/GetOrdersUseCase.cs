using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Dtos.Orders;

namespace OrderFinanceControl.UseCases.Orders;

public class GetOrdersUseCase(IOrderRepository orderRepository)
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<IEnumerable<OrderResponseDto>> ExecuteAsync()
    {
        var orders = await _orderRepository.GetAllAsync();


        return orders.Select(order => new OrderResponseDto
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
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPriceAtOrderTime = i.UnitPrice
            }).ToList()
        });
    }
}
