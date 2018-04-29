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
                            Name = "All products",
                            Action = "Products",
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
                            Name = "All auctions",
                            Action = "Auctions",
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
                    HasChildren = true,
                    Position = 1,
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Level = 2,
                            Name = "Add category",
                            Action = "Categories",
                            Controller = "Admin",
                            Position = 1,
                        },
                        new MenuItem
                        {
                            Level = 2,
                            Name = "All categories",
                            Action = "Categories",
                            Controller = "Admin",
                            Position = 2,
                        }
                    }
                });
                items.Add(new MenuItem
                {
                    Level = 1,
                    Name = "Users",
                    Action = "Users",
                    Controller = "Admin",
                    HasChildren = true,
                    Position = 1,
                    Children = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Level = 2,
                            Name = "Add user",
                            Action = "Users",
                            Controller = "Admin",
                            Position = 1,
                        },
                        new MenuItem
                        {
                            Level = 2,
                            Name = "All users",
                            Action = "Users",
                            Controller = "Admin",
                            Position = 2,
                        }
                    }
                });

                foreach (var item in items)
                {
                    if (item.Action == action && item.Controller == controller)
                        item.IsSelected = true;
                }

                return items;
            }
            catch (Exception)
            {
                return new List<MenuItem>();
            }
        }
        #endregion
    }
}