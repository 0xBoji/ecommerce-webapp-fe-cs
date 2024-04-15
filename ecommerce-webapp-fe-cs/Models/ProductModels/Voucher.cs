namespace ecommerce_webapp_fe_cs.Models.ProductModels;

public class Voucher
{
    public int VoucherId { get; set; }
    public string VoucherName { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Code { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime Expired { get; set; }
    public string? Description { get; set; }
}
