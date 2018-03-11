using JokerKS.WLFU.Entities.Product;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace JokerKS.WLFU.Entities
{
    public class ImageManager
    {
        #region GetById()
        public static Image GetById(int id)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Images.Find(id);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region GetByIds()
        public static List<Image> GetByIds(IEnumerable<int> ids)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Images.Where(x => ids.Contains(x.Id)).ToList();
                }
            }
            catch (Exception)
            {
                return new List<Image>();
            }
        } 
        #endregion
    }
}