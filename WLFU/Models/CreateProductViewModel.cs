using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using WLFU.Entities;

namespace WLFU.Models
{
    public class CreateProductViewModel
    {
        public CreateProductViewModel()
        {
            Images = new List<ImageUploadModel>();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string MainImageIndex { get; set; }

        public IEnumerable<ImageUploadModel> Images { get; set; }
    }

    public class ImageUploadModel
    {
        [Required]
        public HttpPostedFileBase Source { get; set; }
        public string Title { get; set; }
    }
}