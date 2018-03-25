﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace JokerKS.WLFU.Entities.Auction
{
    public static class AuctionManager
    {
        #region GetList()
        public static List<Auction> GetList(Pager pager = null, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.Auctions.AsQueryable();

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
                    var query = db.Auctions.Where(x => x.CategoryId == categoryId).AsQueryable();

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