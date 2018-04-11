using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities.Order
{
    public class ProductOrderDetail
    {
        [Key, Column(Order = 0)]
        public int OrderId { get; set; }
        [Key, Column(Order = 1)]
        public int ProductId { get; set; }

        [Required]
        public int Amount { get; set; }
        [Required]
        public decimal Price { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [ForeignKey("ProductId")]
        public Product.Product Product { get; set; }
    }
}