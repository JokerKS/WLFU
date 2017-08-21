using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Linq;
using WLFU.Entities;
using WLFU.Models;
using System.Web;
using System.IO;
using System;
using System.Net;

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
            return View(new CreateProductViewModel());
        }

        [HttpPost]
        public ActionResult Create(CreateProductViewModel model)
        {


            return View();
        }
    }
}