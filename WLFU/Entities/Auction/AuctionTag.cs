using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities.Auction
{
    public class AuctionTag
    {
        [Key, Column(Order = 0)]
        public int AuctionId { get; set; }
        [Key, Column(Order = 1)]
        public int TagId { get; set; }

        [ForeignKey("AuctionId")]
        public Auction Auction { get; set; }
        [ForeignKey("TagId")]
        public Tag Tag { get; set; }
    }
}