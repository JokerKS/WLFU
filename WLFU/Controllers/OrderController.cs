using JokerKS.WLFU.Models;
using System.Net;
using System.Web.Mvc;

namespace JokerKS.WLFU.Controllers
{
    public class OrderController : Controller
    {
        #region Create()
        [HttpGet]
        public ActionResult Create(OrderModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(model);
        } 
        #endregion
    }
}