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

        public ActionResult Add()
        {
            AppContext db = new AppContext();
            CreateProductModel createmodel = new CreateProductModel();
            createmodel.Product = new Product();
            createmodel.AllTagsString = db.ProductTags.Select(x => x.Tag.Name);
            return View(createmodel);
        }

        [HttpPost]
        public ActionResult Save(CreateProductModel model)
        {
            model.Product.DesignerId = User.Identity.GetUserId();

            #region Save the tags
            if (model.TagsString != null)
            {
                string[] tags = model.TagsString.Split(',');
                if (tags != null && tags.Length > 0 && tags[0] != "")
                {
                    using (AppContext db = AppContext.Create())
                    {
                        model.AllTagsString = db.ProductTags.Select(x => x.Tag.Name);
                        foreach (var item in tags)
                        {
                            if (model.AllTagsString == null || !model.AllTagsString.Contains(item))
                            {
                                model.Product.Tags.Add(new ProductTag { Tag = new Tag() { Name = item } });
                            }
                            else
                            {
                                model.Product.Tags.Add(new ProductTag { TagId = db.Tags.Where(x => x.Name == item).Select(x => x.TagID).SingleOrDefault() });
                            }
                        }
                    }
                }
                else
                    ModelState.AddModelError("Product.Tags", "Tags are required");
            }
            else
                ModelState.AddModelError("Product.Tags", "Tags are required");
            #endregion

            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase fileContent = Request.Files[i];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/Images/"), fileContent.FileName);
                        model.Product.Images.Add(new ProductImage
                        {
                            Image = new Image
                            {
                                Path = "~/Images/" + fileContent.FileName
                            }
                        });

                        fileContent.SaveAs(path);
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            ModelState.Clear();
            TryValidateModel(model);

            if (ModelState.IsValid)
            {
                using (AppContext db = new AppContext())
                {
                    db.Products.Add(model.Product);
                    db.SaveChanges();
                }
            }
            return View(model);
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
            string[] tags;
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

            return View();
        }
    }
}