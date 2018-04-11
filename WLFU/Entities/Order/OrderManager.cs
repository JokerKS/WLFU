using System;

namespace JokerKS.WLFU.Entities.Order
{
    public class OrderManager
    {
        #region Add()
        public static void Add(Order order)
        {
            try
            {
                using (var db = new AppContext())
                {
                    order.DateCreated = DateTime.Now;
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        } 
        #endregion
    }
}