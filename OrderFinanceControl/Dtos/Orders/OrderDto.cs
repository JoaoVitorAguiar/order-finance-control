using System.ComponentModel.DataAnnotations;

namespace OrderFinanceControl.Dtos.Orders;

public record OrderDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
    public int CustomerId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Order must have at least one item.")]
    public List<OrderItemDto> Items { get; set; } = new();
}
