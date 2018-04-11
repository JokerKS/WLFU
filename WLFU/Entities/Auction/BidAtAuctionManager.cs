using System;
using System.Collections.Generic;
using System.Linq;

namespace JokerKS.WLFU.Entities.Auction
{
    public static class BidAtAuctionManager
    {
        #region GetWinningBidByUserId()
        public static List<BidAtAuction> GetWinningBidByUserId(string userId)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Bids.Where(x => x.UserId == userId && x.IsWinner && !x.IsOrdered).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<BidAtAuction>();
            }
        }
        #endregion

        #region GetWinningBid()
        public static BidAtAuction GetWinningBid(int auctionId)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Bids.FirstOrDefault(x => x.AuctionId == auctionId && x.IsWinner && !x.IsOrdered);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Add()
        public static void Add(BidAtAuction bidAtAuction)
        {
            try
            {
                using (var db = new AppContext())
                {
                    bidAtAuction.DateCreated = DateTime.Now;
                    db.Bids.Add(bidAtAuction);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}