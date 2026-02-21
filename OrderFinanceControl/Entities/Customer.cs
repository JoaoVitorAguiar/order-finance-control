namespace OrderFinanceControl.Entities;

public class Customer(string name, string email)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
}
