using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Notification;
using JokerKS.WLFU.Entities.Product;
using JokerKS.WLFU.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace JokerKS.WLFU.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        #region Products() Get
        public ActionResult Products()
        {
            var products = ProductManager.GetNotActiveList();

            return View(products);
        }
        #endregion

        #region ProductDetails() Get
        public ActionResult ProductDetails(int productId)
        {
            AdminProductModel model = new AdminProductModel
            {
                Product = ProductManager.GetById(productId, true)
            };

            if (model.Product != null)
            {
                model.Images = ImageManager.GetByIds(model.Product.Images.Select(x => x.ImageId).ToList());

                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
        #endregion

        #region ProductDetails() Post
        [HttpPost]
        public ActionResult ProductDetails(AdminProductModel model)
        {
            if(model != null && model.Product != null)
            {
                var product = ProductManager.GetById(model.Product.Id);
                product.IsActive = model.Product.IsActive;

                ProductManager.Update(product);

                if(!model.Product.IsActive)
                {
                    var notification = new Notification
                    {
                        UserId = product.DesignerId,
                        Message = model.Message,
                        MessageType = "Product.IsActive"
                    };
                    NotificationManager.Add(notification);

                    // TODO: Відправка емейлу з повідомленням, що продукт не виставлений на продажу і чому

                }

                return RedirectToAction("Products");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion

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