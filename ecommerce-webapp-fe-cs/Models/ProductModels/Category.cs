using System.Collections.Generic;

namespace ecommerce_webapp_fe_cs.Models.ProductModels
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
