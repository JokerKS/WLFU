using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WLFU.Entities
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductTag
    {
        [Key, Column(Order = 0)]
        public int ProductId { get; set; }
        [Key, Column(Order = 1)]
        public int TagId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("TagId")]
        public Tag Tag { get; set; }
    }
}