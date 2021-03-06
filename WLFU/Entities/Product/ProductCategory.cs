﻿using System.ComponentModel.DataAnnotations;

namespace JokerKS.WLFU.Entities.Product
{
    public class ProductCategory : IIdentity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}