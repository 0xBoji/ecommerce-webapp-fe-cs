﻿namespace ecommerce_webapp_fe_cs.Models.ProductModels;

public class Payment
{
    public int PaymentId { get; set; }
    public string OrderId { get; set; } = null!;
    public string PaymentType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? RefundStatus { get; set; }

    public Order Orders { get; set; } = null!;
}