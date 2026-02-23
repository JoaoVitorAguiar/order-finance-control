namespace OrderFinanceControl.Entities;

public class Product(string name, decimal price)
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
}
