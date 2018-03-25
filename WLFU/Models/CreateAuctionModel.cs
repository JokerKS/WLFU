using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JokerKS.WLFU.Models
{
    public class CreateAuctionModel
    {
        public string RequestId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public string MainImageIndex { get; set; }

        public IEnumerable<string> AllTagsString { get; set; }
        [Required]
        [DisplayName("Auction Tags")]
        public string TagsString { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public decimal StartPrice { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public decimal PriceIncrease { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public decimal InstantSellingPrice { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateStart { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateFinish { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        [Required(ErrorMessage = "Please select category")]
        public int CategoryId { get; set; }
    }
}