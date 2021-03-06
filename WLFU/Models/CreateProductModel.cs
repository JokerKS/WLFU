﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JokerKS.WLFU.Models
{
    public class CreateProductModel
    {
        public string RequestId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public string MainImageIndex { get; set; }

        public IEnumerable<string> AllTagsString { get; set; }
        [Required]
        [DisplayName("Product Tags")]
        public string TagsString { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [DefaultValue(1)]
        public short Amount { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        [Required(ErrorMessage = "Please select category")]
        public int CategoryId { get; set; }
    }
}