using OrderFinanceControl.Dtos.Customers;

namespace OrderFinanceControl.Dtos.Orders;
public record OrderResponseDto
{
    public string Id { get; set; } = default!;

    public CustomerResponseDto Customer { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = default!;

    public List<OrderItemResponseDto> Items { get; set; } = [];
}
