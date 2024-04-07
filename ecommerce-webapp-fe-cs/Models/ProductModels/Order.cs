namespace ecommerce_webapp_fe_cs.Models.ProductModels;

public class Order
{
    public string OrderId { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = null!;
    public string? Note { get; set; }
    public decimal TotalPrice { get; set; }
    public int? VoucherId { get; set; }
    public string? ReturnStatus { get; set; }

    public User? Users { get; set; }
    public Voucher Vouchers { get; set; } = null!;
}
