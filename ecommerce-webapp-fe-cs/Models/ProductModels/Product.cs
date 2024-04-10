namespace ecommerce_webapp_fe_cs.Models.ProductModels;
public class Product
{
    public string ProId { get; set; }
    public string ProName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public string Image1 { get; set; }
    public string? Image2 { get; set; }
    public string? Image3 {  get; set; }

    public Category Category { get; set; }
}