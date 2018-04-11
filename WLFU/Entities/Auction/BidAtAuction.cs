using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities.Auction
{
    public class BidAtAuction : IIdentity
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Price { get; set; }
        [Required]
        public bool IsWinner { get; set; }

        // Позначає чи переможець замовив товар
        [Required]
        public bool IsOrdered { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public int AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public Auction Auction { get; set; }
    }
}