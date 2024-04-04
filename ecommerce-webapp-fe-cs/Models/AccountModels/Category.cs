using System.Collections.Generic;
using ecommerce_webapp_fe_cs.Models.ProductModels;

namespace ecommerce_webapp_fe_cs.Models.AccountModels
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
