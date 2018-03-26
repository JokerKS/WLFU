using System;

namespace JokerKS.WLFU.Entities.Auction
{
    public static class BidAtAuctionManager
    {
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