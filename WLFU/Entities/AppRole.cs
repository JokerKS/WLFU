using Microsoft.AspNet.Identity.EntityFramework;

namespace JokerKS.WLFU.Entities
{
    public class AppRole: IdentityRole
    {
        public string Description { get; set; }
    }
}