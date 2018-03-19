using JokerKS.WLFU.Entities.Product;
using JokerKS.WLFU.Models;
using System.Net;
using System.Web.Mvc;
using System.Web.Caching;
using System.Collections.Generic;
using System.Linq;

namespace JokerKS.WLFU.Controllers
{
    public class OrderController : Controller
    {
        #region Create() Get
        [HttpGet]
        public ActionResult Create()
        {
            OrderModel model = (OrderModel)TempData["orderModel"];
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HttpContext.Cache["products"] = model.Products;

            model.Order = new Order();
            return View(model);
        }
        #endregion

        #region Create() Post
        [HttpPost]
        [OutputCache(Duration = 300)]
        public ActionResult Create(OrderModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            model.Products = (Dictionary<SelectedProduct, Product>)HttpContext.Cache["products"];

            if (ModelState.IsValid && model.Products != null && model.Products.Count > 0)
            {
                var products = ProductManager.GetList(model.Products.Select(x => x.Key.ProductId));
                foreach (var prod in products)
                {
                    var key = model.Products.Where(x => x.Key.ProductId == prod.Id).FirstOrDefault().Key;
                    model.Products[key] = prod;
                }

                return View("ShowGeneralInfo", model);
            }

            return View(model);
        }
        #endregion

        //#region ShowGeneralInfo() Get
        //[HttpGet]
        //public ActionResult ShowGeneralInfo(OrderModel model)
        //{
        //    if (model == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }



        //    return View(model);
        //}
        //#endregion
    }
}