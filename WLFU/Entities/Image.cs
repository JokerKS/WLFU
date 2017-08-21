using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WLFU.Entities
{
    public class Image
    {
        [Key]
        public int ImageID { get; set; }
        [Required]
        public string Path { get; set; }
        public string Title { get; set; }
    }

    public class ProductImage
    {
        [Key, Column(Order = 0)]
        public int ProductId { get; set; }
        [Key, Column(Order = 1)]
        public int ImageId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}