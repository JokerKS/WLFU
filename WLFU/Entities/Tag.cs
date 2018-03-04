using System.ComponentModel.DataAnnotations;

namespace JokerKS.WLFU.Entities
{
    public class Tag : IIdentity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }
    }
}