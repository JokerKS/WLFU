﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace JokerKS.WLFU.Entities.Product
{
    public static class ProductManager
    {
        #region GetList()
        public static List<Product> GetList(Pager pager = null, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.Products.AsQueryable();

                    if (includeImages)
                    {
                        query = query.Include(x => x.MainImage);
                    }

                    if (pager != null)
                    {
                        pager.TotalCount = query.Count();

                        if(pager.ItemsSkip > 0)
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
                return new List<Product>();
            }
        }

        public static List<Product> GetList(IEnumerable<int> ids)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Products.Where(x => ids.Contains(x.Id)).ToList();
                }
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }
        #endregion

        #region GetListByCategory()
        public static List<Product> GetListByCategory(int categoryId, Pager pager = null, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.Products.Where(x => x.CategoryId == categoryId).AsQueryable();

                    if(includeImages)
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
                return new List<Product>();
            }
        }
        #endregion

        #region GetById()
        public static Product GetById(int id, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    if (includeImages)
                    {
                        return db.Products
                            .Where(x => x.Id == id)
                            .Include(x => x.MainImage)
                            .Include(x => x.Images)
                            .FirstOrDefault();
                    }
                    else
                    {
                        return db.Products.Find(id);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region Add()
        public static void Add(Product product)
        {
            try
            {
                using (var db = new AppContext())
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update()
        public static void Update(Product product)
        {
            try
            {
                using (var db = new AppContext())
                {
                    db.Products.Attach(product);
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
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
                    db.Products.Remove(db.Products.Find(id));
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