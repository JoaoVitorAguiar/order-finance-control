using System.ComponentModel.DataAnnotations;

namespace OrderFinanceControl.Dtos.Customers;

public record CustomerResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
