using JokerKS.WLFU.Entities.Product;
using JokerKS.WLFU.Models;
using System.Net;
using System.Web.Mvc;
using System.Web.Caching;
using System.Collections.Generic;
using System.Linq;
using JokerKS.WLFU.Entities;
using Microsoft.AspNet.Identity;
using System;
using JokerKS.WLFU.Entities.Auction;
using JokerKS.WLFU.Entities.Order;

namespace JokerKS.WLFU.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        #region Create() Get
        [HttpGet]
        public ActionResult Create()
        {
            // Беремо передані раніше дані
            OrderModel model = (OrderModel)TempData["orderModel"];
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TempData.Remove("orderModel");
            TempData["products"] = model.Products;
            TempData["auctions"] = model.Auctions;

            model.Order = new Order();
            return View(model);
        }
        #endregion

        #region Create() Post
        [HttpPost]
        public ActionResult Create(OrderModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            model.Products = (Dictionary<SelectedProduct, Product>)TempData["products"];
            model.Auctions = (Dictionary<SelectedAuction, Auction>)TempData["auctions"];

            // Якщо дані покупця введені правильно і вибрано товари
            if (ModelState.IsValid && (model.Products != null && model.Products.Count > 0) || 
                (model.Auctions != null && model.Auctions.Count > 0))
            {
                var products = ProductManager.GetList(model.Products.Select(x => x.Key.ProductId));
                foreach (var product in products)
                {
                    var key = model.Products.Where(x => x.Key.ProductId == product.Id).FirstOrDefault().Key;
                    // Добавлення помилки, якщо кількість товарів до замовлення більша ніж доступна
                    if (product.AvailableAmount < key.Amount)
                    {
                        ModelState.AddModelError("", $"The amount of product '{product.Name}' ordered more than is available!");
                    }
                    else
                    {
                        model.Products[key] = product;
                    }
                }

                var auctions = AuctionManager.GetList(model.Auctions.Select(x => x.Key.AuctionId));
                foreach (var auction in auctions)
                {
                    var key = model.Auctions.Where(x => x.Key.AuctionId == auction.Id).FirstOrDefault().Key;
                    model.Auctions[key] = auction;
                }

                // Загрузка зображень до продуктів
                var productImages = model.Products.Where(x => x.Value.MainImageId.HasValue).Select(x => x.Value.MainImageId.Value);
                model.ProductMainImages = ImageManager.GetByIds(productImages).ToDictionary(x => x.Id, v => v);

                var auctionImages = model.Auctions.Where(x => x.Value.MainImageId.HasValue).Select(x => x.Value.MainImageId.Value);
                model.AuctionMainImages = ImageManager.GetByIds(auctionImages).ToDictionary(x => x.Id, v => v);

                // Підрахунок суми замовлення
                model.SummaryPrice = 0M;
                foreach (var item in model.Products)
                {
                    model.SummaryPrice += item.Key.Amount * item.Value.Price;
                }

                foreach (var item in model.Auctions)
                {
                    model.SummaryPrice += item.Value.CurrentPrice;
                }

                if (ModelState.IsValid)
                {
                    TempData["products"] = model.Products;
                    TempData["auctions"] = model.Auctions;
                    return View("ShowGeneralInfo", model);
                }
            }

            return View(model);
        }
        #endregion

        #region ShowGeneralInfo() Post
        [HttpPost]
        public ActionResult ShowGeneralInfo(OrderModel model)
        {
            model.Products = (Dictionary<SelectedProduct, Product>)TempData["products"];
            model.Auctions = (Dictionary<SelectedAuction, Auction>)TempData["auctions"];

            // Виведення помилки, якщо:
            // 1. модель пуста
            // 2. дані замовника пусті
            // 3. немає даних про товари і аукціони
            if (model == null || model.Order == null || 
                ((model.Products == null || model.Products.Count < 1) && (model.Auctions == null || model.Auctions.Count < 1)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var products = ProductManager.GetList(model.Products.Select(x => x.Key.ProductId));
                    // Перевірка чи доступні всі продукти, які користувач хоче замовити
                    if (model.Products.Count < products.Count)
                    {
                        ModelState.AddModelError("", $"Some product is no longer available to order!");
                    }
                    foreach (var product in products)
                    {
                        var key = model.Products.Where(x => x.Key.ProductId == product.Id).FirstOrDefault().Key;
                        // Добавлення помилки, якщо кількість товарів до замовлення більша ніж доступна
                        if (product.AvailableAmount < key.Amount)
                        {
                            ModelState.AddModelError("", $"The amount of product '{product.Name}' ordered more than is available!");
                        }
                        else
                        {
                            model.Products[key] = product;
                        }
                    }

                    // Збереження замовлення і його деталей
                    model.Order.UserId = User.Identity.GetUserId();
                    model.Order.ProductDetails = new List<ProductOrderDetail>();
                    foreach (var item in model.Products)
                    {
                        model.Order.ProductDetails.Add(new ProductOrderDetail
                            {
                                Price = item.Value.Price,
                                Amount = item.Key.Amount,
                                ProductId = item.Value.Id
                            }
                        );
                    }

                    // TODO: check auctions
                    model.Order.AuctionDetails = new List<AuctionOrderDetail>();
                    foreach (var item in model.Auctions)
                    {
                        model.Order.AuctionDetails.Add(new AuctionOrderDetail
                            {
                                Price = item.Value.CurrentPrice,
                                Amount = 1,
                                AuctionId = item.Value.Id
                            }
                        );
                    }

                    OrderManager.Add(model.Order);

                    // Видалення замовлених товарів з кошика
                    foreach (var item in model.Order.ProductDetails)
                    {
                        BasketProductManager.Delete(model.Order.UserId, item.ProductId);
                    }

                    foreach (var item in model.Order.AuctionDetails)
                    {
                        AuctionManager.MarkAuctionAsOrdered(item.AuctionId, User.Identity.GetUserId());
                    }

                    return View("Result", model.Order.Id);
                }
                catch(Exception ex)
                {

                }
            }

            // Загрузка зображень до продуктів
            var images = model.Products.Where(x => x.Value.MainImageId.HasValue).Select(x => x.Value.MainImageId.Value);
            model.ProductMainImages = ImageManager.GetByIds(images).ToDictionary(x => x.Id, v => v);

            var auctionImages = model.Auctions.Where(x => x.Value.MainImageId.HasValue).Select(x => x.Value.MainImageId.Value);
            model.AuctionMainImages = ImageManager.GetByIds(auctionImages).ToDictionary(x => x.Id, v => v);

            // Підрахунок суми замовлення
            model.SummaryPrice = 0M;
            foreach (var item in model.Products)
            {
                model.SummaryPrice += item.Key.Amount * item.Value.Price;
            }

            foreach (var item in model.Auctions)
            {
                model.SummaryPrice += item.Value.CurrentPrice;
            }

            TempData["products"] = model.Products;
            TempData["auctions"] = model.Auctions;
            return View(model);
        }
        #endregion
    }
}