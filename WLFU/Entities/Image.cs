using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities
{
    public class Image : IIdentity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Path { get; set; }
        public string Title { get; set; }
    }
}