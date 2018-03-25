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

namespace JokerKS.WLFU.Controllers
{
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

            // Якщо дані покупця введені правильно і вибрано товари
            if (ModelState.IsValid && model.Products != null && model.Products.Count > 0)
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

                // Загрузка зображень до продуктів
                var images = model.Products.Where(x => x.Value.MainImageId.HasValue).Select(x => x.Value.MainImageId.Value);
                model.MainImages = ImageManager.GetByIds(images).ToDictionary(x => x.Id, v => v);

                // Підрахунок суми замовлення
                model.SummaryPrice = 0M;
                foreach (var item in model.Products)
                {
                    model.SummaryPrice += item.Key.Amount * item.Value.Price;
                }

                if (ModelState.IsValid)
                {
                    TempData["products"] = model.Products;
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
            if (model == null || model.Products == null || model.Order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var products = ProductManager.GetList(model.Products.Select(x => x.Key.ProductId));
            // Перевірка чи доступні всі продукти, які користувач хоче замовити
            if(model.Products.Count < products.Count)
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
            if (ModelState.IsValid)
            {
                try
                {
                    // Збереження замовлення і його деталей
                    model.Order.UserId = User.Identity.GetUserId();
                    model.Order.Details = new List<OrderDetail>();
                    foreach (var item in model.Products)
                    {
                        model.Order.Details.Add(new OrderDetail
                            {
                                Price = item.Value.Price,
                                Amount = item.Key.Amount,
                                ProductId = item.Value.Id
                            }
                        );
                    }
                    OrderManager.Add(model.Order);

                    // Видалення замовлених товарів з кошика
                    foreach (var item in model.Order.Details)
                    {
                        BasketProductManager.Delete(model.Order.UserId, item.ProductId);
                    }

                    return View("Result", model.Order.Id);
                }
                catch(Exception ex)
                {

                }
            }

            // Загрузка зображень до продуктів
            var images = model.Products.Where(x => x.Value.MainImageId.HasValue).Select(x => x.Value.MainImageId.Value);
            model.MainImages = ImageManager.GetByIds(images).ToDictionary(x => x.Id, v => v);

            // Підрахунок суми замовлення
            model.SummaryPrice = 0M;
            foreach (var item in model.Products)
            {
                model.SummaryPrice += item.Key.Amount * item.Value.Price;
            }

            TempData["products"] = model.Products;
            return View(model);
        }
        #endregion
    }
}