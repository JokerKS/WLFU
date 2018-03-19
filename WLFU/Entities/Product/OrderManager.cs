using JokerKS.WLFU.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JokerKS.WLFU.Entities.Product
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