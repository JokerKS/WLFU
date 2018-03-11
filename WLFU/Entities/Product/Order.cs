using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities.Product
{
    public class Order : IIdentity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public List<OrderDetail> Details { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }
    }
}