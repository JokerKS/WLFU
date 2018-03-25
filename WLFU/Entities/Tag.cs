using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokerKS.WLFU.Entities
{
    public class Tag : IIdentity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
    }
}