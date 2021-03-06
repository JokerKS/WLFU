﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using JokerKS.WLFU.Entities.Helpers;

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
                            .Include(x => x.Designer)
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
        public static List<Auction> GetList()
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Auctions.OrderBy(x => x.Id).ToList();
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

        #region GetAvailableList()
        public static List<Auction> GetAvailableList(Pager pager = null, int? categoryId = null, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.Auctions.Include(x => x.Tags).Where(x => !x.IsClosed && x.IsActive);
                    if (categoryId.HasValue && categoryId.Value > 0)
                    {
                        query = query.Where(x => x.CategoryId == categoryId.Value);
                    }
                    if (includeImages)
                    {
                        query = query.Include(x => x.MainImage);
                    }

                    var auctions = query.ToList().AsEnumerable();

                    if (pager != null)
                    {
                        if (!string.IsNullOrEmpty(pager.SearchExpression))
                        {
                            var search = pager.SearchExpression;
                            var tagIds = TagManager.GetList().Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).Select(x => x.Id).ToList();

                            auctions = auctions.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                x.Description.Contains(search, StringComparison.OrdinalIgnoreCase) || x.Tags.Where(z => tagIds.Contains(z.TagId)).Count() > 0);
                        }

                        if (!string.IsNullOrEmpty(pager.SortQuery))
                        {
                            auctions = auctions.OrderBy(pager.SortQuery);
                        }
                        else
                        {
                            auctions = auctions.OrderBy(x => x.Id);
                        }

                        pager.TotalCount = auctions.Count();

                        if (pager.ItemsSkip > 0)
                        {
                            auctions = auctions.Skip(pager.ItemsSkip);
                        }
                        auctions = auctions.Take(pager.ItemsPerPage);
                    }

                    return auctions.ToList();
                }
            }
            catch (Exception)
            {
                return new List<Auction>();
            }
        }
        #endregion

        #region GetNotActiveList()
        public static List<Auction> GetNotActiveList()
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Auctions.Where(x => !x.IsActive)
                        .Include(x => x.Designer)
                        .Include(x => x.Category)
                        .OrderBy(x => x.DateModified).ToList();
                }
            }
            catch (Exception)
            {
                return new List<Auction>();
            }
        }
        #endregion

        #region GetListByDesigner()
        public static List<Auction> GetListByDesigner(string designerId, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.Auctions.Where(x => x.DesignerId == designerId).AsQueryable();

                    if (includeImages)
                    {
                        query = query.Include(x => x.MainImage);
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