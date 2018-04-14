using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace JokerKS.WLFU.Entities.Product
{
    public static class ProductCategoryManager
    {
        #region GetList()
        public static List<ProductCategory> GetList(Pager pager = null)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.ProductCategories.AsQueryable();

                    if (pager != null)
                    {
                        pager.TotalCount = query.Count();

                        if(pager.ItemsSkip > 0)
                        {
                            query = query.Skip(pager.ItemsSkip);
                        }
                        query = query.Take(pager.ItemsPerPage);
                    }

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                return new List<ProductCategory>();
            }
        }
        #endregion

        #region GetById()
        public static ProductCategory GetById(int id)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.ProductCategories.Find(id);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region Add()
        public static void Add(ProductCategory category)
        {
            try
            {
                using (var db = new AppContext())
                {
                    db.ProductCategories.Add(category);
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
        public static void Update(ProductCategory category)
        {
            try
            {
                using (var db = new AppContext())
                {
                    db.ProductCategories.Attach(category);
                    db.Entry(category).State = EntityState.Modified;
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
                    db.ProductCategories.Remove(db.ProductCategories.Find(id));
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