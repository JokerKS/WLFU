using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Auction;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class AuctionModel
    {
        public Auction Auction { get; set; }
        public List<Image> Images { get; set; }
    }
}