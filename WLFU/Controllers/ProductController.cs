using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Linq;
using WLFU.Entities;
using WLFU.Models;
using System.Web;
using System.IO;
using System;
using System.Net;
using System.Collections.Generic;

namespace WLFU.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            CreateProductViewModel createmodel = new CreateProductViewModel();
            using (var db = new AppContext())
                createmodel.AllTagsString = db.ProductTags.Select(x => x.Tag.Name).ToList();

            return View(createmodel);
        }

        [HttpPost]
        public ActionResult Create(CreateProductViewModel model, IEnumerable<HttpPostedFileBase> files, IEnumerable<string> titles)
        {
            #region Get tags from string
            string[] tags = null;
            if (!string.IsNullOrEmpty(model.TagsString))
            {
                tags = model.TagsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if(tags.Length == 0)
                    ModelState.AddModelError("TagsString", "Product Tags are required");
            }
            else
                ModelState.AddModelError("TagsString", "Product Tags are required");
            #endregion

            #region Images check
            if (files == null || files.Count() == 0)
                ModelState.AddModelError("Images", "Required at least one image");
            if (model.MainImageIndex == null)
                ModelState.AddModelError("MainImageIndex", "It should be noted the main image");
            #endregion

            if (!ModelState.IsValid)
            {
                using (var db = new AppContext())
                {
                    model.AllTagsString = db.ProductTags.Select(x => x.Tag.Name).ToList();
                }

                return View(model);
            }

            Product prod = new Product() {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Amount = model.Amount,
                DesignerId = User.Identity.GetUserId(),
            };

            using (var db = new AppContext())
            {
                //get request id of product creation
                ProductCreationRequest req;
                if (model.RequestId == null)
                {
                    req = new ProductCreationRequest() { ProductId = null };
                    db.ProductCreationRequests.Add(req);
                    db.SaveChanges();
                }
                else
                    req = db.ProductCreationRequests.Find(model.RequestId);

                //work with images
                List<Image> images = new List<Image>();
                string path = Server.MapPath("~/Images/Products/") + req.RequestId + '/';
                try
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    //save images and get paths
                    foreach (var file in files.Select((value, index) => new { index, value }))
                    {
                        string fileName = Path.GetFileName(file.value.FileName);
                        file.value.SaveAs(path + fileName);

                        if (file.index == Convert.ToInt32(model.MainImageIndex))
                        {
                            prod.MainImage = new Image()
                            {
                                Path = req.RequestId + "/" + fileName,
                                Title = titles.ElementAt(file.index)
                            };
                        }
                        else
                        {
                            images.Add(new Image()
                            {
                                Path = req.RequestId + "/" + fileName,
                                Title = titles.ElementAt(file.index)
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                prod.Images = images.Select(x => new ProductImage() { Image = x }).ToList();

                foreach (var tag in tags)
                {
                    var existTag = db.Tags.FirstOrDefault(x => x.Name == tag);
                    if (existTag == null)
                        prod.Tags.Add(new ProductTag() { Tag = new Tag() { Name = tag } });
                    else
                        prod.Tags.Add(new ProductTag() { Tag = existTag });
                }

                db.Products.Add(prod);
                db.SaveChanges();

                using (var dbCtx = new AppContext())
                {
                    req.ProductId = prod.ProductId;
                    dbCtx.Entry(req).State = System.Data.Entity.EntityState.Modified;
                    dbCtx.SaveChanges();
                }

                return View("Success", req.RequestId);
            }
        }
    }
}