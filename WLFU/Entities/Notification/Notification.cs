using JokerKS.WLFU.Entities.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace JokerKS.WLFU.Entities.Notification
{
    public class Notification : IIdentity
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        [AllowHtml]
        public string Message { get; set; }
        [Required]
        [StringLength(100)]
        public string Action { get; set; }
        public int MessageType { get; set; }
        public System.DateTime DateCreated { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }
    }

    public enum MessageType
    {
        Info,
        Warning,
        Success
    }
}