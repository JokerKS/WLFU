using JokerKS.WLFU.Entities.Helpers;
using JokerKS.WLFU.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace JokerKS.WLFU.Entities.Auction
{
    public class Auction : IIdentity
    {
        public Auction()
        {
            Tags = new List<AuctionTag>();
            Images = new List<AuctionImage>();
        }

        [Key]
        [Sortable("Date Created")]
        public int Id { get; set; }
        [Required]
        [Sortable]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Is published")]
        public bool IsPublished { get; set; }
        [Required]
        [Display(Name = "Is active")]
        public bool IsActive { get; set; }
        [Required]
        [Display(Name = "Is closed")]
        public bool IsClosed { get; set; }

        [Required]
        [Display(Name = "Start price")]
        public decimal StartPrice { get; set; }
        [Required]
        [Display(Name = "Price increase")]
        public decimal PriceIncrease { get; set; }
        [Display(Name = "Instant selling price")]
        public decimal? InstantSellingPrice { get; set; }

        public DateTime? DateModified { get; set; }

        #region DateStart
        private DateTime? dateStart;

        [Display(Name = "Start date")]
        public DateTime? DateStart
        {
            get
            {
                return dateStart;
            }
            set
            {
                if (value == null || value.Value < new DateTime(2018, 1, 1))
                {
                    dateStart = null;
                }
                else
                {
                    dateStart = value;
                }
            }
        } 
        #endregion

        [Required]
        [Sortable("Finish Date")]
        [Display(Name = "Finish Date")]
        public DateTime DateFinish { get; set; }

        #region Designer
        [Required]
        [Sortable("Designer")]
        public string DesignerId { get; set; }
        [ForeignKey("DesignerId")]
        public AppUser Designer { get; set; }
        #endregion

        #region Tags, Images, Comments
        [Required]
        public IList<AuctionTag> Tags { get; set; }
        public IList<AuctionImage> Images { get; set; }
        public IList<AuctionComment> Comments { get; set; }
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
        public Product.ProductCategory Category { get; set; }
        #endregion

        [NotMapped]
        [Sortable("Price")]
        [Display(Name = "Current price")]
        public decimal CurrentPrice
        {
            get
            {
                return AuctionManager.GetCurrentPrice(this);
            }
        }
    }
}