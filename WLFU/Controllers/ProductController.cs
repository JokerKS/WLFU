using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.IO;
using System;
using System.Collections.Generic;
using JokerKS.WLFU.Models;
using JokerKS.WLFU.Entities;
using JokerKS.WLFU;
using System.Data.Entity;
using JokerKS.WLFU.Entities.Product;
using System.Net;
using JokerKS.WLFU.Entities.Helpers;
using JokerKS.WLFU.Entities.Auction;
using JokerKS.WLFU.Entities.User;

namespace JokeKS.WLFU.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        #region Create() Get
        [HttpGet]
        public ActionResult Create()
        {
            CreateProductModel model = new CreateProductModel();
            model.Categories = ProductCategoryManager.GetList()
                .Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });

            model.AllTagsString = TagManager.GetList().Select(x => x.Name);

            return View(model);
        }
        #endregion

        #region Create() Post
        [HttpPost]
        public ActionResult Create(CreateProductModel model, IEnumerable<string> filePathes, IEnumerable<string> titles)
        {
            #region Get tags from string
            string[] tags = null;
            if (!string.IsNullOrEmpty(model.TagsString))
            {
                tags = model.TagsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (tags.Length == 0)
                    ModelState.AddModelError("TagsString", "Product Tags are required");
            }
            else
                ModelState.AddModelError("TagsString", "Product Tags are required");
            #endregion

            #region Images check
            if (filePathes == null || filePathes.Count() < 1)
            {
                ModelState.AddModelError("Images", "Required at least one image");
            }
            if (model.MainImageIndex == null)
            {
                ModelState.AddModelError("MainImageIndex", "It should be noted the main image");
            }
            #endregion

            if (model.RequestId == null)
            {
                ModelState.AddModelError("RequestId", "RequestId can't be null");
            }

            #region Return view if Form not valid
            if (!ModelState.IsValid)
            {
                model.AllTagsString = TagManager.GetList().Select(x => x.Name);

                return View(model);
            }
            #endregion

            try
            {
                Product prod = new Product()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    Amount = model.Amount,
                    DesignerId = User.Identity.GetUserId(),
                    CategoryId = model.CategoryId
                };

                using (var db = new AppContext())
                {
                    foreach (var tag in tags)
                    {
                        var existTag = TagManager.GetByName(tag);
                        if (existTag == null)
                            prod.Tags.Add(new ProductTag() { Tag = new Tag() { Name = tag } });
                        else
                            prod.Tags.Add(new ProductTag() { TagId = existTag.Id });
                    }

                    ProductManager.Add(prod, db);

                    //work with images
                    List<Image> images = new List<Image>();
                    string path = Server.MapPath("~/Images/Temp/Products/") + model.RequestId + '\\';
                    string pathToMove = Server.MapPath("~/Images/Products/") + prod.Id + '\\';

                    if (!Directory.Exists(pathToMove))
                        Directory.CreateDirectory(pathToMove);

                    //save images and get paths
                    foreach (var file in filePathes.Select((value, index) => new { index, value }))
                    {
                        string fileName = file.value;
                        if (System.IO.File.Exists(path + fileName))
                        {
                            System.IO.File.Move(path + fileName, pathToMove + fileName);

                            if (file.index == Convert.ToInt32(model.MainImageIndex))
                            {
                                prod.MainImage = new Image()
                                {
                                    Path = prod.Id + "/" + fileName,
                                    Title = titles.ElementAt(file.index)
                                };
                            }
                            else
                            {
                                prod.Images.Add(new ProductImage
                                {
                                    Image = new Image
                                    {
                                        Path = prod.Id + "/" + fileName,
                                        Title = titles.ElementAt(file.index)
                                    }
                                });
                            }
                        }
                    }

                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }

                    ProductManager.Update(prod, db);

                    var successModel = new SuccessCreateProductModel()
                    {
                        ProductName = prod.Name
                    };
                    return View("Success", successModel);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region SaveImages() Post
        [HttpPost]
        public JsonResult SaveImages(IEnumerable<HttpPostedFileBase> images, string requestId)
        {
            try
            {
                Guid guid = Guid.NewGuid();
                if (!string.IsNullOrEmpty(requestId))
                {
                    guid = new Guid(requestId);
                }

                string basePath = Server.MapPath("~/Images/Temp/Products/") + guid + "\\";
                if (Directory.Exists(basePath))
                {
                    Directory.Delete(basePath, true);
                }
                Directory.CreateDirectory(basePath);

                //save images and get paths
                var pathes = new List<string>();
                string fileName = string.Empty;
                foreach (var image in images.Select((value, index) => new { index, value }))
                {
                    fileName = Path.GetFileName(image.value.FileName);
                    pathes.Add(fileName);
                    image.value.SaveAs($"{basePath}{fileName}");
                }

                return Json(new { message = "Uploaded successfully", requestId = guid.ToString(), pathes = pathes });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Uploaded error" });
            }
        }
        #endregion


        #region List() Get
        [HttpGet]
        [AllowAnonymous]
        public ActionResult List(Pager pager = null, int? categoryId = null)
        {
            if (pager == null)
            {
                pager = new Pager();
            }
            pager.ItemsPerPage = 10;

            var model = new ProductListModel { Pager = pager };

            // Беремо всі категорії + додаємо допоміжну, яка значить вибрати всі категорії
            model.Categories = new List<ProductCategory>();
            model.Categories.Add(new ProductCategory() { Id = 0, Name = "Wszystkie" });
            model.Categories.AddRange(ProductCategoryManager.GetList());

            model.SortExpressions = new Dictionary<string, string>();
            model.SortExpressions.Add(string.Empty, string.Empty);
            IEnumerable<Sortable> sortAttr;
            string propertyName;
            foreach (var propertyInfo in typeof(Product).GetProperties()
                    .Where(x => x.IsSortable()).Select(x => x))
            {
                sortAttr = propertyInfo.GetCustomAttributes(false).Where(x => x.GetType() == typeof(Sortable)).Select(x => x as Sortable ?? null);
                if(sortAttr.Count() > 0)
                {
                    propertyName = sortAttr.First() == null ? null : sortAttr.First().SortablePropertyName;
                    model.SortExpressions.Add(propertyInfo.Name, string.IsNullOrEmpty(propertyName) ? propertyInfo.Name : propertyName);
                }
            }

            // Вибираємо продукти за поданими фільтрами
            model.Products = ProductManager.GetAvailableList(pager, categoryId, true);

            return View(model);
        }
        #endregion

        #region Details() Get
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(int productId)
        {
            ProductModel model = new ProductModel();

            model.Product = ProductManager.GetById(productId, true);
            if(model.Product != null)
            {
                model.AddedToBasket = UserManager.GetBasketProduct(User.Identity.GetUserId(), model.Product.Id);
                model.Images = ImageManager.GetByIds(model.Product.Images.Select(x => x.ImageId).ToList());

                // Якщо даний товар доданий в цього користувача вже доданий до кошика, 
                // То віднімаємо від кількості доступних товарів до покупки кількість в кошику
                model.AvailableAmount = model.Product.AvailableAmount - (model.AddedToBasket != null ? model.AddedToBasket.Amount : 0);

                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion

        #region AddToBasket() Get
        [HttpGet]
        [AllowAnonymous]
        public JsonResult AddToBasket(int productId, int amount)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    redirectUrl = (Url.Action("Login", "Account",
                        new {
                            ReturnUrl = Url.Action("Details", "Product", new { productId = productId })
                        }))
                }, JsonRequestBehavior.AllowGet);
            }

            var model = new MessageResult();

            try
            {
                // Якщо продукт не знайдено(наприклад його видалили зняли з продажі),
                // То не дозволяємо додавати продукт до кошика
                var product = ProductManager.GetById(productId);
                if (product == null)
                {
                    throw new Exception("Product not founded!");
                }

                // Якщо доступна кількість продукту меньша ніж додавана до кошика,
                // То не дозволяємо додавати продукт до кошика
                // Не забуваємо враховувати кількість для цього користувача вже доданих до кошика
                var existedInBasket = BasketProductManager.GetBasketProduct(User.Identity.GetUserId(), product.Id);

                if (product.AvailableAmount - (existedInBasket != null ? existedInBasket.Amount : 0) < amount)
                {
                    throw new Exception("You cannot add more than available to order!");
                }

                var basketProduct = new BasketProduct()
                {
                    ProductId = productId,
                    UserId = User.Identity.GetUserId(),
                    Amount = amount,
                    DateCreated = DateTime.Now
                };

                BasketProductManager.Add(basketProduct);

                // Продукт успішно доданий до кошика 
                model.Message =  $"Product \"{product.Name}\" was successfully added to the basket in the amounts of {amount}";
                model.Succeeded = true;
            }
            catch (Exception ex)
            {
                // Додаємо повідомлення з помилкою
                model.Message = "Product wasn't added to basket";
                model.Error = ex.Message;
                model.Succeeded = false;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DeleteFromBasket() Get
        public ActionResult DeleteFromBasket(int? productId)
        {
            var model = new MessageResult();
            try
            {
                // Перевіряємо чи подано productId
                if ((productId ?? 0) == 0)
                {
                    throw new Exception("Product not founded!");
                }

                // Перевіряємо чи є такий продукт
                var product = ProductManager.GetById(productId.Value);
                if(product == null)
                {
                    throw new Exception("Product not founded!");
                }

                BasketProductManager.Delete(User.Identity.GetUserId(), productId.Value);

                // Продукт успішно видалений з кошика 
                model.Message = $"Product \"{product.Name}\" was successfully deleted from your basket!";
                model.Succeeded = true;
            }
            catch (Exception ex)
            {
                // Додаємо повідомлення з помилкою
                model.Message = "An error occurred while deleting the item from your basket!";
                model.Error = ex.Message;
                model.Succeeded = false;
            }
            return RedirectToAction("Basket", new { message = model });
        } 
        #endregion

        #region Basket() Get
        [HttpGet]
        public ActionResult Basket(MessageResult deleteResult)
        {
            BasketListModel model = new BasketListModel();
            model.DeleteResult = deleteResult;

            // Беремо всі продукти в кошику користувача
            model.ProductsInBasket = BasketProductManager.GetProductsInBasket(User.Identity.GetUserId());

            // Позначаємо всі продукти зазначеними
            model.SelectedProducts = model.ProductsInBasket.Select(x => new SelectedProduct
            {
                ProductId = x.ProductId,
                Checked = true,
                Amount = x.Amount
            }).ToList();

            // Беремо всі виграні аукціони
            model.Bids = BidAtAuctionManager.GetWinningBidByUserId(User.Identity.GetUserId());
            model.AuctionInBasket = AuctionManager.GetList(model.Bids.Select(x => x.AuctionId));

            // Позначаємо всі аукціони зазначеними
            model.SelectedAuctions = model.AuctionInBasket.Select(x => new SelectedAuction
            {
                AuctionId = x.Id,
                Checked = true
            }).ToList();

            model.SummaryPrice = 0M;
            foreach (var item in model.ProductsInBasket)
            {
                model.SummaryPrice += item.Amount * item.Product.Price;
            }

            foreach (var item in model.AuctionInBasket)
            {
                model.SummaryPrice += item.CurrentPrice;
            }

            var productImages = model.ProductsInBasket.Where(x => x.Product.MainImageId.HasValue).Select(x => x.Product.MainImageId.Value);
            model.ProductMainImages = ImageManager.GetByIds(productImages).ToDictionary(x => x.Id, v => v);

            var auctionImages = model.AuctionInBasket.Where(x => x.MainImageId.HasValue).Select(x => x.MainImageId.Value);
            model.AuctionMainImages = ImageManager.GetByIds(auctionImages).ToDictionary(x => x.Id, v => v);

            return View(model);
        }
        #endregion

        #region Basket() Post
        [HttpPost]
        public ActionResult Basket(BasketListModel model)
        {
            // Якщо модель пуста
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Якщо не зазначено жодного продукту або аукціону
            else if (model.SelectedProducts == null && model.SelectedAuctions == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (!model.SelectedProducts.Any() && !model.SelectedAuctions.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Беремо всі зазначені товари, щоб перевірити чи можна їх замовити
            var selectedProducts = model.SelectedProducts.Where(x => x.Checked && x.Amount > 0).ToDictionary(x => x.ProductId, x => x.Amount);
            var products = ProductManager.GetList(selectedProducts.Select(x => x.Key));
            foreach (var product in products)
            {
                // Добавлення помилки, якщо кількість товарів до замовлення більша ніж доступна
                if(product.AvailableAmount < selectedProducts[product.Id])
                {
                    ModelState.AddModelError("", $"The amount of product '{product.Name}' ordered more than is available!");
                }
            }

            if (ModelState.IsValid)
            {
                var orderModel = new OrderModel();
                orderModel.Products = new Dictionary<SelectedProduct, Product>();
                foreach (var item in model.SelectedProducts.Where(x => x.Checked))
                {
                    orderModel.Products.Add(item, null);
                }

                orderModel.Auctions = new Dictionary<SelectedAuction, Auction>();
                foreach (var item in model.SelectedAuctions.Where(x => x.Checked))
                {
                    orderModel.Auctions.Add(item, null);
                }

                // Для передачі до іншого контролера зберігаємо дані до TempData
                TempData["orderModel"] = orderModel;

                // Якщо все добре, то переходимо до оформлення замовлення
                return RedirectToAction("Create", "Order");
            }

            // Беремо всі продукти в кошику користувача
            model.ProductsInBasket = BasketProductManager.GetProductsInBasket(User.Identity.GetUserId());
            // Беремо всі виграні аукціони
            model.AuctionInBasket = AuctionManager.GetList(model.Bids.Select(x => x.AuctionId));

            var productImages = model.ProductsInBasket.Where(x => x.Product.MainImageId.HasValue).Select(x => x.Product.MainImageId.Value);
            model.ProductMainImages = ImageManager.GetByIds(productImages).ToDictionary(x => x.Id, v => v);

            var auctionImages = model.AuctionInBasket.Where(x => x.MainImageId.HasValue).Select(x => x.MainImageId.Value);
            model.AuctionMainImages = ImageManager.GetByIds(auctionImages).ToDictionary(x => x.Id, v => v);

            return View(model);
        }
        #endregion
    }
}