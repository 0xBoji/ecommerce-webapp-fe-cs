namespace ecommerce_webapp_fe_cs.Models.ProductModels;

public class OrderItem
{
    public string OrderItemId { get; set; } = null!;
    public string OrderId { get; set; } = null!;
    public string ProId { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Order Orders { get; set; } = null!;
    public Product Products { get; set; } = null!;
}