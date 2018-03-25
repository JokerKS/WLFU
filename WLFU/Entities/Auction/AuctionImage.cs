using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities.Auction
{
    public class AuctionImage
    {
        [Key, Column(Order = 0)]
        public int AuctionId { get; set; }
        [Key, Column(Order = 1)]
        public int ImageId { get; set; }

        [ForeignKey("AuctionId")]
        public Auction Auction { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}