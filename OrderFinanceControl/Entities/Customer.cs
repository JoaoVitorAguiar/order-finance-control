namespace OrderFinanceControl.Entities;

public class Customer(string name, string email)
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
}