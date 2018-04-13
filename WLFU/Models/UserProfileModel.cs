using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Auction;
using JokerKS.WLFU.Entities.Product;
using JokerKS.WLFU.Entities.User;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class UserProfileModel
    {
        public AppUser User { get; set; }
        public IList<Product> UserProducts { get; set; }
        public IList<Auction> UserAuctions { get; set; }

        public Dictionary<int, Image> ProductMainImages { get; set; }
        public Dictionary<int, Image> AuctionMainImages { get; set; }
    }
}