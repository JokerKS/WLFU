using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class ProductListModel
    {
        public ProductListModel()
        {
            Products = new List<Product>();
            Categories = new List<ProductCategory>();
        }

        public List<Product> Products { get; set; }
        public Pager Pager { get; set; }

        public List<ProductCategory> Categories { get; set; }
        public int CategoryId { get; set; }
    }
}