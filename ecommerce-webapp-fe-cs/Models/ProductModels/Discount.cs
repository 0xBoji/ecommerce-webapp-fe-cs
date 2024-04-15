namespace ecommerce_webapp_fe_cs.Models.ProductModels;

public class Discount
{
    public int DiscountId { get; set; }
    public int CategoryId { get; set; }
    public string DiscountType { get; set; } = null!;
    public decimal DiscountValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }

    public Category Categories { get; set; } = null!;
}
