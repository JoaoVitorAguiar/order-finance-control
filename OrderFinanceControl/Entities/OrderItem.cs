namespace OrderFinanceControl.Entities;

public class OrderItem
{
    public OrderItem() { }
    public OrderItem(int productId, decimal unitPriceAtOrderTime, int quantity)
    {
        ProductId = productId;
        UnitPriceAtOrderTime = unitPriceAtOrderTime;
        Quantity = quantity;
    }

    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } 
    public int ProductId { get; set; }
    public Product Product { get; set; } 
    public int Quantity { get; set; }
    public decimal UnitPriceAtOrderTime { get; set; }
}
