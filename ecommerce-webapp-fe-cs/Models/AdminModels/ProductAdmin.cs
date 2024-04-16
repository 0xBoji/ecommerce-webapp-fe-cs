using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ecommerce_webapp_fe_cs.Models.AdminModels;


    public class ProductAdmin
    {
        public string ProName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile ProImg1 { get; set; }
        public IFormFile ProImg2 { get; set; }
        public IFormFile ProImg3 { get; set; }
        public int StockQuantity { get; set; }
    }

