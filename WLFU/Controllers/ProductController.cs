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

namespace JokeKS.WLFU.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
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
                                prod.Images.Add(new ProductImage
                                {
                                    Image = new Image
                                    {
                                        Path = req.RequestId + "/" + fileName,
                                        Title = titles.ElementAt(file.index)
                                    }
                                });
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    foreach (var tag in tags)
                    {
                        var existTag = db.Tags.FirstOrDefault(x => x.Name == tag);
                        if (existTag == null)
                            prod.Tags.Add(new ProductTag() { Tag = new Tag() { Name = tag } });
                        else
                            prod.Tags.Add(new ProductTag() { TagId = existTag.TagID });
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}