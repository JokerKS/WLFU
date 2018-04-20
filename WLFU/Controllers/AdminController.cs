using JokerKS.WLFU.Entities.Product;
using System.Web.Mvc;

namespace JokerKS.WLFU.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        public ActionResult Products()
        {
            var products = ProductManager.GetNotActiveList();

            return View(products);
        }

        public ActionResult Auctions()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }
    }
}