using JokerKS.WLFU.Entities.Product;
using JokerKS.WLFU.Models;
using System.Net;
using System.Web.Mvc;

namespace JokerKS.WLFU.Controllers
{
    public class OrderController : Controller
    {
        #region Create()
        [HttpGet]
        public ActionResult Create()
        {
            OrderModel model = (OrderModel)TempData["orderModel"];
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            model.Order = new Order();
            return View(model);
        } 
        #endregion
    }
}