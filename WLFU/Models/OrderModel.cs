using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class OrderModel
    {
        public Order Order { get; set; }
        public List<Product> Products { get; set; }
    }
}