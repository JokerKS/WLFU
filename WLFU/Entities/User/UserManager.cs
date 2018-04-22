using JokerKS.WLFU.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JokerKS.WLFU.Entities.User
{
    public static class UserManager
    {
        #region GetList()
        public static List<AppUser> GetList()
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Users.ToList();
                }
            }
            catch (Exception)
            {
                return new List<AppUser>();
            }
        }
        #endregion

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
    }
}