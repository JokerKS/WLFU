using JokerKS.WLFU.Entities.User;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities.Auction
{
    public class AuctionComment : IIdentity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Text { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public int AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public Auction Auction { get; set; }
    }
}