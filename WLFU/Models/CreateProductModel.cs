using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WLFU.Entities;

namespace WLFU.Models
{
    public class CreateProductModel
    {
        [Required]
        public Product Product { get; set; }
        public IEnumerable<string> AllTagsString { get; set; }
        [Required]
        public string TagsString { get; set; }
    }
}