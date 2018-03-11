using JokerKS.WLFU.Entities.Product;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace JokerKS.WLFU.Entities
{
    public class UserManager
    {
        #region GetBasketProduct()
        public static BasketProduct GetBasketProduct(string userId, int productId)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.BasketProducts.Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region AddProductToBasket()
        public static void AddProductToBasket(BasketProduct model)
        {
            try
            {
                using (var db = new AppContext())
                {
                    var existed = GetBasketProduct(model.UserId, model.ProductId);
                    if (existed != null)
                    {
                        existed.Amount = existed.Amount + model.Amount;
                        db.Entry(existed).State = EntityState.Modified;
                    }
                    else
                    {
                        db.BasketProducts.Add(model);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region RemoveProductFromBasket()
        public static void RemoveProductFromBasket(string userId, int productId)
        {
            try
            {
                using (var db = new AppContext())
                {
                    db.Entry(db.BasketProducts.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId))
                        .State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region GetProductsInBasket()
        public static List<BasketProduct> GetProductsInBasket(string userId)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.BasketProducts.Where(x => x.UserId == userId)
                        .Select(x => new
                        {
                            p = x,
                            r = x.Product
                        })
                        .AsEnumerable()
                        .Select(x => x.p)
                        .OrderBy(x => x.DateCreated)
                        .ToList();
                }
            }
            catch (Exception)
            {
                return new List<BasketProduct>();
            }
        } 
        #endregion
    }
}