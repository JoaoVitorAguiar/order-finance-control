using OrderFinanceControl.Dtos.Customers;

namespace OrderFinanceControl.Dtos.Orders;
public record OrderResponseDto
{
    public int Id { get; set; }

    public CustomerResponseDto Customer { get; set; }

    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }

    public List<OrderItemResponseDto> Items { get; set; } = [];
}