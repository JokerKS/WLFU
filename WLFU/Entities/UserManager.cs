using JokerKS.WLFU.Entities.Product;
using System;
using System.Linq;

namespace JokerKS.WLFU.Entities
{
    public static class UserManager
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
    }
}