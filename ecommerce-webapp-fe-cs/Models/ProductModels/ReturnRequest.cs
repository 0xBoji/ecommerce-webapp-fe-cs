namespace ecommerce_webapp_fe_cs.Models.ProductModels;
public class ReturnRequest
{
    public int RequestId { get; set; }
    public string OrderId { get; set; } = null!;
    public string? Reason { get; set; }
    public string RequestStatus { get; set; } = null!;
    public DateTime RequestDate { get; set; }

    public Order Orders { get; set; } = null!;
}