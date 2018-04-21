using System;

namespace JokerKS.WLFU.Entities.Notification
{
    public static class NotificationManager
    {
        #region Add()
        public static void Add(Notification notification)
        {
            try
            {
                using (var db = new AppContext())
                {
                    notification.DateCreated = DateTime.Now;
                    db.Notifications.Add(notification);
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