using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Auction;
using JokerKS.WLFU.Entities.Order;
using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class OrderModel
    {
        public Order Order { get; set; }

        public Dictionary<SelectedProduct, Product> Products { get; set; }
        public Dictionary<int, Image> ProductMainImages { get; set; }

        public Dictionary<SelectedAuction, Auction> Auctions { get; set; }
        public Dictionary<int, Image> AuctionMainImages { get; set; }

        public decimal SummaryPrice { get; set; }
    }
}