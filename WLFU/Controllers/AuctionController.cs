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
using JokerKS.WLFU.Entities.Product;
using System.Net;
using JokerKS.WLFU.Entities.Helpers;
using JokerKS.WLFU.Entities.Auction;
using System.Transactions;

namespace JokeKS.WLFU.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {
        #region Create() Get
        [HttpGet]
        public ActionResult Create()
        {
            CreateAuctionModel model = new CreateAuctionModel();
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
        public ActionResult Create(CreateAuctionModel model, IEnumerable<string> filePathes, IEnumerable<string> titles)
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
                Auction auction = new Auction()
                {
                    Name = model.Name,
                    Description = model.Description,
                    DateStart = model.DateStart,
                    DateFinish = model.DateFinish,
                    StartPrice = model.StartPrice,
                    PriceIncrease = model.PriceIncrease,
                    InstantSellingPrice = model.InstantSellingPrice,
                    DesignerId = User.Identity.GetUserId(),
                    CategoryId = model.CategoryId
                };

                using (var db = new AppContext())
                {
                    foreach (var tag in tags)
                    {
                        var existTag = TagManager.GetByName(tag);
                        if (existTag == null)
                            auction.Tags.Add(new AuctionTag() { Tag = new Tag() { Name = tag } });
                        else
                            auction.Tags.Add(new AuctionTag() { TagId = existTag.Id });
                    }
                    AuctionManager.Add(auction, db);

                    //work with images
                    List<Image> images = new List<Image>();
                    string path = Server.MapPath("~/Images/Temp/Auctions/") + model.RequestId + '\\';
                    string pathToMove = Server.MapPath("~/Images/Auctions/") + auction.Id + '\\';

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
                                auction.MainImage = new Image()
                                {
                                    Path = auction.Id + "/" + fileName,
                                    Title = titles.ElementAt(file.index)
                                };
                            }
                            else
                            {
                                auction.Images.Add(new AuctionImage
                                {
                                    Image = new Image
                                    {
                                        Path = auction.Id + "/" + fileName,
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

                    AuctionManager.Update(auction, db);
                }

                var successModel = new SuccessCreateAuctionModel()
                {
                    AuctionName = auction.Name
                };
                return View("Success", successModel);
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

                string basePath = Server.MapPath("~/Images/Temp/Auctions/") + guid + "\\";
                if (Directory.Exists(basePath))
                {
                    Directory.Delete(basePath, true);
                }
                Directory.CreateDirectory(basePath);

                // Збереження зображень і отримання шляху до них
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
        public ActionResult List(int page = 1, int? categoryId = null)
        {
            var pager = new Pager(page);

            var model = new AuctionListModel()
            {
                Pager = pager,
                Categories = ProductCategoryManager.GetList()
            };

            if(categoryId.HasValue && categoryId > 0)
            {
                model.Auctions = AuctionManager.GetListByCategory(categoryId.Value, pager, true);
            }
            else
            {
                model.Auctions = AuctionManager.GetList(pager, true);
            }

            return View(model);
        }
        #endregion

        #region Details() Get
        [HttpGet]
        public ActionResult Details(int auctionId)
        {
            AuctionModel model = new AuctionModel();

            model.Auction = AuctionManager.GetById(auctionId, true);
            if(model.Auction != null)
            {
                model.Images = ImageManager.GetByIds(model.Auction.Images.Select(x => x.ImageId).ToList());

                return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        #endregion

        #region MakeBid() Get
        [HttpGet]
        public JsonResult MakeBid(int auctionId, decimal price)
        {
            var model = new MessageResult();

            try
            {
                // Якщо аукціон не знайдено(наприклад його видалили),
                var auction = AuctionManager.GetById(auctionId);
                if (auction == null)
                {
                    throw new Exception("Auction not founded!");
                }

                if(auction.IsClosed)
                {
                    throw new Exception($"The auction '{auction.Name}' is completed! Make a bid is no longer possible.");
                }

                // Перевіряємо чи дана ставка можлива
                if(auction.CurrentPrice + auction.PriceIncrease > price)
                {
                    throw new Exception($"The price '{price}' can't be less than the minimum price of the last bid '{auction.CurrentPrice + auction.PriceIncrease}'");
                }

                var bid = new BidAtAuction()
                {
                    AuctionId = auction.Id,
                    Price = price,
                    UserId = User.Identity.GetUserId()
                };

                // Використовуємо TransactionScope, щоб в базі одночасно занести дані ставки 
                // і обновити дані аукціону, якщо це фінальна ставка
                using (TransactionScope scope = new TransactionScope())
                {
                    // Якщо в аукціоні вказана ціна, по якій завершується аукціон і ставка перевищує або = цій ціні, 
                    // то позначаємо аукціон завершеним, а ставку кінцевою(користувач виграв лот)
                    if (auction.InstantSellingPrice.HasValue && bid.Price >= auction.InstantSellingPrice.Value)
                    {
                        bid.IsWinner = true;
                        auction.IsClosed = true;
                        AuctionManager.Update(auction);
                    }

                    BidAtAuctionManager.Add(bid);

                    scope.Complete();
                }
                // Ставка успішно зроблена
                model.Message = $"Bid at the auction \"{auction.Name}\" was successfully added!";
                if(bid.IsWinner)
                {
                    model.Message += "\nCongratulations! Your bid won on the auction!\nTo place your order go to basket";
                }
                model.Succeeded = true;
            }
            catch (Exception ex)
            {
                // Не вдалося зробити ставку
                model.Message = "Bid at the auction wasn't added!";
                model.Error = ex.Message;
                model.Succeeded = false;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}