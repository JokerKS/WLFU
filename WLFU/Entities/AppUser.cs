using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JokerKS.WLFU.Entities
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public System.DateTime BirthDate { get; set; }

        public List<Product.BasketProduct> BasketProducts { get; set; }
        public List<Order.Order> Orders { get; set; }
    }
}