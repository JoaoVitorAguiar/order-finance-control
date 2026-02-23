using System.ComponentModel.DataAnnotations;

namespace OrderFinanceControl.Dtos.Products;
public record ProductDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; }

    [Required]
    [Range(0.01, 999999999)]
    public decimal Price { get; set; }
}