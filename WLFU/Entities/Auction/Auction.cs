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
        public bool IsPublished { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsClosed { get; set; }

        [Required]
        public decimal StartPrice { get; set; }
        [Required]
        public decimal PriceIncrease { get; set; }
        public decimal? InstantSellingPrice { get; set; }

        #region DateStart
        private DateTime? dateStart;

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
        public decimal CurrentPrice
        {
            get
            {
                return AuctionManager.GetCurrentPrice(this);
            }
        }
    }
}