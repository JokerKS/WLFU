using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities.Product
{
    public class ProductComment : IIdentity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Text { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}