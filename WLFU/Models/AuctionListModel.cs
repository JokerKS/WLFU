using JokerKS.WLFU.Entities.Auction;
using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class AuctionListModel
    {
        public AuctionListModel()
        {
            Auctions = new List<Auction>();
            Categories = new List<ProductCategory>();
        }

        public List<Auction> Auctions { get; set; }
        public Pager Pager { get; set; }

        public List<ProductCategory> Categories { get; set; }
        public int CategoryId { get; set; }

        public Dictionary<string, string> SortExpressions { get; set; }
    }
}