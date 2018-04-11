using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace JokerKS.WLFU.Entities.Auction
{
    public static class AuctionManager
    {
        #region GetById()
        public static Auction GetById(int id, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    if (includeImages)
                    {
                        return db.Auctions
                            .Where(x => x.Id == id)
                            .Include(x => x.MainImage)
                            .Include(x => x.Images)
                            .FirstOrDefault();
                    }
                    else
                    {
                        return db.Auctions.Find(id);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region GetList()
        public static List<Auction> GetList(Pager pager = null, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.Auctions.Where(x => !x.IsClosed);

                    if (includeImages)
                    {
                        query = query.Include(x => x.MainImage);
                    }

                    if (pager != null)
                    {
                        pager.TotalCount = query.Count();

                        if (pager.ItemsSkip > 0)
                        {
                            query = query.Skip(pager.ItemsSkip);
                        }
                        query = query.Take(pager.PageSize);
                    }

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                return new List<Auction>();
            }
        }

        public static List<Auction> GetList(IEnumerable<int> ids)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Auctions.Where(x => ids.Contains(x.Id)).ToList();
                }
            }
            catch (Exception)
            {
                return new List<Auction>();
            }
        }
        #endregion

        #region GetListByCategory()
        public static List<Auction> GetListByCategory(int categoryId, Pager pager = null, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.Auctions.Where(x => x.CategoryId == categoryId && !x.IsClosed).AsQueryable();

                    if (includeImages)
                    {
                        query = query.Include(x => x.MainImage);
                    }

                    if (pager != null)
                    {
                        pager.TotalCount = query.Count();

                        if (pager.ItemsSkip > 0)
                        {
                            query = query.Skip(pager.ItemsSkip);
                        }
                        query = query.Take(pager.PageSize);
                    }

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                return new List<Auction>();
            }
        }
        #endregion


        #region MarkAuctionAsOrdered
        public static void MarkAuctionAsOrdered(int auctionId, string userId)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var bid = BidAtAuctionManager.GetWinningBid(auctionId);
                    if(bid == null || bid.UserId != userId)
                    {
                        throw new Exception("This user won't the auction");
                    }

                    // Позначаємо ставку як замовлена
                    bid.IsOrdered = true;
                    db.Entry(bid).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        } 
        #endregion



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


        #region GetCurrentPrice()
        public static decimal GetCurrentPrice(Auction auction)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var lastBid = db.Bids.Where(x => x.AuctionId == auction.Id).OrderByDescending(x => x.Price).FirstOrDefault();
                    if(lastBid != null)
                    {
                        return lastBid.Price;
                    }
                    else
                    {
                        return auction.StartPrice;
                    }
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