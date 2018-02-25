using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities
{
    public class ProductCreationRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid RequestId { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}