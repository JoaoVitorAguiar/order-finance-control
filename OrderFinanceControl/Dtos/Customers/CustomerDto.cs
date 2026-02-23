using System.ComponentModel.DataAnnotations;

namespace OrderFinanceControl.Dtos.Customers;


public record CustomerDto
{
    [Required]
    [MaxLength(150)]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public required string Email { get; set; }
}