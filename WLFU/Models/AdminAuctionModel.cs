using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Auction;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JokerKS.WLFU.Models
{
    public class AdminAuctionModel
    {
        public Auction Auction { get; set; }
        public List<Image> Images { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}