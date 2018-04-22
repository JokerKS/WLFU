using System.Web.Mvc;
using System.Web.Routing;

namespace JokerKS.WLFU
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Admin",
                "Admin/Products",
                new { controller = "Admin", action = "Products" }
            );
            routes.MapRoute(
                "Products",
                "Products/{id}",
                new { controller = "Product", action = "List", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Auctions",
                "Auctions/{id}",
                new { controller = "Auction", action = "List", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Basket",
                "Basket",
                new { controller = "Product", action = "Basket"}
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
