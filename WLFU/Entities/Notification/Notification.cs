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
        [StringLength(100)]
        public string MessageType { get; set; }
        public System.DateTime DateCreated { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }
    }
}