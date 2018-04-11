using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Auction;
using JokerKS.WLFU.Entities.Helpers;
using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class BasketListModel
    {
        public IEnumerable<BasketProduct> ProductsInBasket { get; set; }
        public Dictionary<int, Image> ProductMainImages { get; set; }
        public List<SelectedProduct> SelectedProducts { get; set; }

        public IEnumerable<Auction> AuctionInBasket { get; set; }
        public List<BidAtAuction> Bids { get; set; }
        public Dictionary<int, Image> AuctionMainImages { get; set; }
        public List<SelectedAuction> SelectedAuctions { get; set; }


        public decimal SummaryPrice { get; set; }

        public MessageResult DeleteResult { get; set; }
    }

    public class SelectedProduct
    {
        public int ProductId { get; set; }
        public bool Checked { get; set; }
        public int Amount { get; set; }
    }

    public class SelectedAuction
    {
        public int AuctionId { get; set; }
        public bool Checked { get; set; }
    }
}