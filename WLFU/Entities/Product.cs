using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace JokerKS.WLFU.Entities
{
    public class Product
    {
        public Product()
        {
            Tags = new List<ProductTag>();
            Images = new List<ProductImage>();
        }

        [Key]
        public int ProductId { get; set; }
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

        [Required]
        public IList<ProductTag> Tags { get; set; }
        public IList<ProductImage> Images { get; set; }

        [Required]
        public int MainImageId { get; set; }
        [ForeignKey("MainImageId")]
        public Image MainImage { get; set; }

        /*
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        */
    }

    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
    }

    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        [Required]
        public string CommentText { get; set; }
    }
}