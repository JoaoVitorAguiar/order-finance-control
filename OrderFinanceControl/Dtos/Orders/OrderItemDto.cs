using System.ComponentModel.DataAnnotations;

namespace OrderFinanceControl.Dtos.Orders;

public record OrderItemDto
{
    [Required]
    public string ProductId { get; set; } = default!;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}
