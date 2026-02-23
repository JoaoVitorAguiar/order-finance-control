namespace OrderFinanceControl.Entities;

public class OrderItem(
    string productId,
    string productName,
    decimal unitPrice,
    int quantity)
{
    public string ProductId { get; set; } = productId;
    public string ProductName { get; set; } = productName;
    public decimal UnitPrice { get; set; } = unitPrice;
    public int Quantity { get; set; } = quantity;
}
