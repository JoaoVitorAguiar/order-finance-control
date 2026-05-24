using OrderFinanceControl.Dtos.Customers;

namespace OrderFinanceControl.Dtos.Orders;

public record OrderItemResponseDto
{
    public string ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;

    public int Quantity { get; set; }
    public decimal UnitPriceAtOrderTime { get; set; }
}
