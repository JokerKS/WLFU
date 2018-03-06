using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            Images = new List<Image>();
        }
        public Product Product { get; set; }
        public List<Image> Images { get; set; }
    }
}