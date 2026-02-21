using OrderFinanceControl.Enums;

namespace OrderFinanceControl.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public DateTime? PaidAt { get; set; }
    public ICollection<OrderItem> Items { get; set; } = [];
}
