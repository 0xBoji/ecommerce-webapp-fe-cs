namespace ecommerce_webapp_fe_cs.Models.ProductModels;

public class Refund
{
    public int RefundId { get; set; }
    public string OrderId { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? RefundMethod { get; set; }
    public string RefundStatus { get; set; } = null!;
    public DateTime RefundDate { get; set; }

    public Order Order { get; set; } = null!;
}
