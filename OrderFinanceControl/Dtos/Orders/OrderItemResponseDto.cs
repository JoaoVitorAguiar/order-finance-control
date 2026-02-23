using OrderFinanceControl.Dtos.Customers;

namespace OrderFinanceControl.Dtos.Orders;

public record OrderItemResponseDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPriceAtOrderTime { get; set; }
}
