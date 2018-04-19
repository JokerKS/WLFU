using JokerKS.WLFU.Entities.User;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace JokerKS.WLFU.Entities.Product
{
    public class Product : IIdentity
    {
        #region Product()
        public Product()
        {
            Tags = new List<ProductTag>();
            Images = new List<ProductImage>();
        } 
        #endregion

        #region Product
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
        public bool IsPublished { get; set; }
        [Required]
        public bool IsActive { get; set; } 
        #endregion

        #region Designer
        [Required]
        public string DesignerId { get; set; }
        [ForeignKey("DesignerId")]
        public AppUser Designer { get; set; } 
        #endregion

        #region Tags, Images, Comments, Ratings
        [Required]
        public IList<ProductTag> Tags { get; set; }
        public IList<ProductImage> Images { get; set; }
        public IList<ProductComment> Comments { get; set; }
        public IList<ProductRating> Ratings { get; set; }
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


        [NotMapped]
        public int AvailableAmount
        {
            get
            {
                var amount = 0;
                amount = ProductManager.GetAllowedCount(this);

                return amount;
            }
        }
    }
}