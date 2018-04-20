using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;

namespace JokerKS.WLFU.Entities.Product
{
    public static class ProductManager
    {
        #region GetList()
        public static List<Product> GetAvailableList(Pager pager = null, int? categoryId = null, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.Products.Include(x => x.Tags).AsQueryable();
                    if(categoryId.HasValue)
                    {
                        query = query.Where(x => x.CategoryId == categoryId.Value);
                    }
                    if (includeImages)
                    {
                        query = query.Include(x => x.MainImage);
                    }

                    // Вибираємо тільки ті продукти, яких кількість "на складі" більша 0
                    var products = query.ToList().Where(x => x.AvailableAmount > 0);

                    if (pager != null)
                    {
                        if(!string.IsNullOrEmpty(pager.SearchExpression))
                        {
                            var search = pager.SearchExpression;
                            var tagIds = TagManager.GetList().Where(x => x.Name.Contains(search)).Select(x => x.Id).ToList();

                            products = products.Where(x => x.Name.Contains(search) ||
                                x.Description.Contains(search) || x.Tags.Where(z => tagIds.Contains(z.TagId)).Count() > 0);                   
                        }

                        if (!string.IsNullOrEmpty(pager.SortQuery))
                        {
                            products = products.OrderBy(pager.SortQuery);
                        }
                        else
                        {
                            products = products.OrderBy(x => x.Id);
                        }

                        pager.TotalCount = products.Count();

                        if(pager.ItemsSkip > 0)
                        {
                            products = products.Skip(pager.ItemsSkip);
                        }
                        products = products.Take(pager.ItemsPerPage);
                    }

                    return products.ToList();
                }
            }
            catch (Exception e)
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

        #region GetListByDesigner()
        public static List<Product> GetListByDesigner(string designerId, bool includeImages = false)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var query = db.Products.Where(x => x.DesignerId == designerId).AsQueryable();

                    if (includeImages)
                    {
                        query = query.Include(x => x.MainImage);
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
                            .Include(x => x.Designer)
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
        public static void Add(Product product, AppContext context = null)
        {
            try
            {
                if (context == null)
                {
                    using (var db = new AppContext())
                    {
                        db.Products.Add(product);
                        db.SaveChanges();
                    }
                }
                else
                {
                    context.Products.Add(product);
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
        public static void Update(Product product, AppContext context = null)
        {
            try
            {
                if(context == null)
                { 
                    using (var db = new AppContext())
                    {
                        db.Products.Attach(product);
                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    context.Entry(product).State = EntityState.Modified;
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


        #region GetAllowedCount()
        public static int GetAllowedCount(Product product)
        {
            var count = 0;
            try
            {
                using (var db = new AppContext())
                {
                    var orders = db.ProductOrderDetails.Where(x => x.ProductId == product.Id);
                    var orderedCount = 0;
                    if(orders.Count() > 0)
                    {
                        orderedCount = orders.Sum(x => x.Amount);
                    }
                    count = product.Amount - orderedCount;
                }
                if (count < 0)
                    throw new Exception("The Count cannot be negative");
            }
            catch (Exception)
            {
                count = 0;
            }
            return count;
        } 
        #endregion
    }
}