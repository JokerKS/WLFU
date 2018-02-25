using System.Web.Mvc;

namespace JokeKS.WLFU.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}