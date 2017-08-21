using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace WLFU.Entities
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public System.DateTime BirthDate { get; set; }
    }
}