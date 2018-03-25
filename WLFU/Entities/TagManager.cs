using System;
using System.Collections.Generic;
using System.Linq;

namespace JokerKS.WLFU.Entities
{
    public class TagManager
    {
        #region GetList()
        public static List<Tag> GetList()
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Tags.ToList();
                }
            }
            catch (Exception)
            {
                return new List<Tag>();
            }
        }
        #endregion

        #region GetByName()
        public static Tag GetByName(string name)
        {
            try
            {
                using (var db = new AppContext())
                {
                    return db.Tags.FirstOrDefault(x => x.Name == name);
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