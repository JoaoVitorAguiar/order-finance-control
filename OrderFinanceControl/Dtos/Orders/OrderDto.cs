using System.ComponentModel.DataAnnotations;

namespace OrderFinanceControl.Dtos.Orders;

public record OrderDto
{
    [Required]
    public string CustomerId { get; set; } = default!;

    [Required]
    [MinLength(1, ErrorMessage = "Order must have at least one item.")]
    public List<OrderItemDto> Items { get; set; } = new();
}
