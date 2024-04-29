using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ecommerce_webapp_fe_cs.Models.ProductModels;
public class Product
{
    public string ProId { get; set; }
    public string ProName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ProImg1 { get; set; }
    public string ProImg2 { get; set; }
    public string ProImg3 { get; set; }
    public int StockQuantity { get; set; }
}