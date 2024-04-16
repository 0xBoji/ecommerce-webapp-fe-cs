namespace ecommerce_webapp_fe_cs.Models.ProductModels;

public class CartItemModel
{
    public string OrderItemId { get; set; }
    public string OrderId { get; set; }
    public string ProId { get; set; }
    public string ProName { get; set; }
    public string Description { get; set; }
    public string ProImg1 { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Price * Quantity;
}
