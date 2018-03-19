using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class OrderModel
    {
        public Order Order { get; set; }
        public Dictionary<SelectedProduct, Product> Products { get; set; }
    }
}