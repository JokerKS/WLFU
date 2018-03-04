using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JokerKS.WLFU.Models
{
    public class ProductCategoryAddModel
    {
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}