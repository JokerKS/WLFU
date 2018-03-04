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

namespace JokeKS.WLFU.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        #region Create() Get
        [HttpGet]
        public ActionResult Create()
        {
            CreateProductModel createmodel = new CreateProductModel();
            using (var db = new AppContext())
                createmodel.AllTagsString = db.ProductTags.Select(x => x.Tag.Name).ToList();

            return View(createmodel);
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
                using (var db = new AppContext())
                {
                    model.AllTagsString = db.ProductTags.Select(x => x.Tag.Name).ToList();
                }

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
                };

                using (var db = new AppContext())
                {
                    foreach (var tag in tags)
                    {
                        var existTag = db.Tags.FirstOrDefault(x => x.Name == tag);
                        if (existTag == null)
                            prod.Tags.Add(new ProductTag() { Tag = new Tag() { Name = tag } });
                        else
                            prod.Tags.Add(new ProductTag() { TagId = existTag.Id });
                    }

                    db.Products.Add(prod);
                    db.SaveChanges();

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
                                    Path = prod.Id + '/' + fileName,
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

                    db.Entry<Product>(prod).State = EntityState.Modified;
                    db.SaveChanges();

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

        #region Success() Get
        [HttpGet]
        private ViewResult Success(SuccessCreateProductModel model)
        {
            return View(model);
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
            var id = Convert.ToInt32(categoryId);
            var model = new ProductCategoryAddModel();
            if (id > 0)
            {
                var category = ProductCategoryManager.GetById(id);
                if(category != null)
                {
                    model.CategoryId = category.Id;
                    model.Name = category.Name;
                    model.Description = category.Description;
                }
            }

            return PartialView(model);
        }
        #endregion

        #region CategoryAdd() Post
        [HttpPost]
        public ActionResult CategoryAdd(ProductCategoryAddModel model)
        {
            if(model != null)
            {
                var category = new ProductCategory();
                category.Name = model.Name;
                category.Description = model.Description;

                if(model.CategoryId > 0)
                {
                    category.Id = model.CategoryId;
                    ProductCategoryManager.Update(category);
                }
                else
                {
                    ProductCategoryManager.Add(category);
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


        #region List() Get
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        #endregion
    }
}