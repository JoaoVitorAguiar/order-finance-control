using OrderFinanceControl.Enums;
using System.Text.Json.Serialization;

namespace OrderFinanceControl.Entities;

public class Order(Customer customer, IEnumerable<OrderItem> items)
{
    public string Id { get; set; } = default!;
    public Customer Customer { get; set; } = customer;
    public List<OrderItem> Items { get; set; } = items.ToList();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount =>
        Items.Sum(i => i.Quantity * i.UnitPrice);
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public DateTime? PaidAt { get; set; }
}
