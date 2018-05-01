using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JokerKS.WLFU.Entities.Menu
{
    public static class MenuManager
    {
        #region GetMenuItems()
        public static List<MenuItem> GetMenuItems(string action, string controller)
        {
            try
            {
                var urlHelper = new UrlHelper();

                var items = new List<MenuItem>();
                items.Add(new MenuItem
                {
                    Level = 1,
                    Name = "Products",
                    Action = "Products",
                    Controller = "Admin",
                    HasChildren = true,
                    Position = 1,
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Level = 2,
                            Name = "Add product",
                            Action = "Products",
                            Controller = "Admin",
                            Position = 1,
                        },
                        new MenuItem
                        {
                            Level = 2,
                            Name = "Products To Check",
                            Action = "ProductsToCheck",
                            Controller = "Admin",
                            Position = 2,
                        }
                    }
                });
                items.Add(new MenuItem
                {
                    Level = 1,
                    Name = "Auctions",
                    Action = "Auctions",
                    Controller = "Admin",
                    HasChildren = true,
                    Position = 1,
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Level = 2,
                            Name = "Add auction",
                            Action = "Auctions",
                            Controller = "Admin",
                            Position = 1,
                        },
                        new MenuItem
                        {
                            Level = 2,
                            Name = "Auctions To Check",
                            Action = "AuctionsToCheck",
                            Controller = "Admin",
                            Position = 2,
                        }
                    }
                });
                items.Add(new MenuItem
                {
                    Level = 1,
                    Name = "Categories",
                    Action = "Categories",
                    Controller = "Admin",
                    Position = 3,
                });
                items.Add(new MenuItem
                {
                    Level = 1,
                    Name = "Users",
                    Action = "Users",
                    Controller = "Admin",
                    Position = 4,
                });

                FindSelected(items, action, controller);

                return items;
            }
            catch (Exception)
            {
                return new List<MenuItem>();
            }
        }
        #endregion

        #region FindSelected() private
        private static void FindSelected(IEnumerable<MenuItem> items, string action, string controller)
        {
            bool selectedFound = false;
            foreach (var item in items)
            {
                if (item.Action == action && item.Controller == controller)
                {
                    item.IsSelected = selectedFound = true;
                    break;
                }
                else if (item.Children != null)
                {
                    FindSelected(item.Children, action, controller);
                }
                if (selectedFound)
                {
                    break;
                }
            }
        } 
        #endregion
    }
}