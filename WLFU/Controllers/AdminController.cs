using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Auction;
using JokerKS.WLFU.Entities.Notification;
using JokerKS.WLFU.Entities.Product;
using JokerKS.WLFU.Entities.User;
using JokerKS.WLFU.Models;
using System;
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

        #region Auctions() Get
        public ActionResult Auctions()
        {
            var auctions = AuctionManager.GetNotActiveList();

            return View(auctions);
        }
        #endregion

        #region AuctionDetails() Get
        public ActionResult AuctionDetails(int auctionId)
        {
            AdminAuctionModel model = new AdminAuctionModel
            {
                Auction = AuctionManager.GetById(auctionId, true)
            };

            if (model.Auction != null)
            {
                model.Images = ImageManager.GetByIds(model.Auction.Images.Select(x => x.ImageId).ToList());

                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
        #endregion

        #region AuctionDetails() Post
        [HttpPost]
        public ActionResult AuctionDetails(AdminAuctionModel model)
        {
            if (model != null && model.Auction != null)
            {
                var auction = AuctionManager.GetById(model.Auction.Id);
                auction.IsActive = model.Auction.IsActive;

                AuctionManager.Update(auction);

                if (!model.Auction.IsActive)
                {
                    var notification = new Notification
                    {
                        UserId = auction.DesignerId,
                        Message = model.Message,
                        MessageType = "Auction.IsActive"
                    };
                    NotificationManager.Add(notification);

                    // TODO: Відправка емейлу з повідомленням, що продукт не виставлений на продажу і чому

                }

                return RedirectToAction("Auctions");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Categories() Get
        [HttpGet]
        public ActionResult Categories(ProductCategoryListModel model = null)
        {
            if (model == null)
            {
                var pager = new Pager();

                model = new ProductCategoryListModel()
                {
                    Pager = pager
                };
            }
            model.Categories = ProductCategoryManager.GetList(model.Pager);

            return View(model);
        }
        #endregion

        #region CategoryAdd() Get
        [HttpGet]
        public ActionResult CategoryAdd(string categoryId)
        {
            var category = ProductCategoryManager.GetById(Convert.ToInt32(categoryId));
            if (category == null)
            {
                category = new ProductCategory();
            }

            return PartialView("_CategoryAdd", category);
        }
        #endregion

        #region CategoryAdd() Post
        [HttpPost]
        public ActionResult CategoryAdd(ProductCategory model)
        {
            if (model != null)
            {
                if (model.Id > 0)
                {
                    ProductCategoryManager.Update(model);
                }
                else
                {
                    ProductCategoryManager.Add(model);
                }
                return RedirectToAction("Categories");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        #endregion

        #region CategoryDelete() Get
        [HttpGet]
        public ActionResult CategoryDelete(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategoryManager.Delete(id.Value);
            return RedirectToAction("Categories");
        }
        #endregion

        #region Users() Get
        public ActionResult Users()
        {
            var users = UserManager.GetList();
            return View(users);
        } 
        #endregion
    }
}