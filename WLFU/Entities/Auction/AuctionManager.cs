using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace JokerKS.WLFU.Entities.Auction
{
    public static class AuctionManager
    {

        #region Add()
        public static void Add(Auction auction, AppContext context = null)
        {
            try
            {
                if (context == null)
                {
                    using (var db = new AppContext())
                    {
                        db.Auctions.Add(auction);
                        db.SaveChanges();
                    }
                }
                else
                {
                    context.Auctions.Add(auction);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update()
        public static void Update(Auction auction, AppContext context = null)
        {
            try
            {
                if (context == null)
                {
                    using (var db = new AppContext())
                    {
                        db.Auctions.Attach(auction);
                        db.Entry(auction).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    context.Entry(auction).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete()
        public static void Delete(int id)
        {
            try
            {
                using (var db = new AppContext())
                {
                    db.Auctions.Remove(db.Auctions.Find(id));
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