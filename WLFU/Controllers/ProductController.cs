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