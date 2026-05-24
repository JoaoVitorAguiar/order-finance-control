using System.ComponentModel.DataAnnotations;

namespace OrderFinanceControl.Dtos.Customers;

public record CustomerResponseDto
{
    public string Id { get; set; } = default!;
    public required string Name { get; set; }
}
