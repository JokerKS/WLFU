using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace JokerKS.WLFU.Entities.Product
{
    public class Product : IIdentity
    {
        public Product()
        {
            Tags = new List<ProductTag>();
            Images = new List<ProductImage>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DefaultValue(1)]
        public short Amount { get; set; }

        [Required]
        public string DesignerId { get; set; }
        [ForeignKey("DesignerId")]
        public AppUser Designer { get; set; }

        #region Tags and Images
        [Required]
        public IList<ProductTag> Tags { get; set; }
        public IList<ProductImage> Images { get; set; } 
        #endregion

        #region MainImage
        public int? MainImageId { get; set; }
        [ForeignKey("MainImageId")]
        public Image MainImage { get; set; } 
        #endregion

        #region Category
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public ProductCategory Category { get; set; } 
        #endregion
    }
}