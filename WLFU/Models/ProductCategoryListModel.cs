using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class ProductCategoryListModel
    {
        public ProductCategoryListModel()
        {
            Categories = new List<ProductCategory>();
        }

        public List<ProductCategory> Categories { get; set; }
        public Pager Pager { get; set; }
    }
}