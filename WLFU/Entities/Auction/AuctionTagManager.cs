using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace JokerKS.WLFU.Entities.Auction
{
    public static class AuctionTagManager
    {
        #region GetList()
        public static List<AuctionTag> GetList()
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.AuctionTags.ToList();
                }
            }
            catch (Exception)
            {
                return new List<AuctionTag>();
            }
        }
        #endregion
    }
}