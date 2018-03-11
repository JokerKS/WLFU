using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class ProductModel
    {
        public Product Product { get; set; }
        public List<Image> Images { get; set; }
        public BasketProduct AddedToBasket { get; set; }
        public int AvailableAmount { get; set; }
    }
}