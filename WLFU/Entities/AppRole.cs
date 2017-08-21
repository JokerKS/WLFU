using Microsoft.AspNet.Identity.EntityFramework;

namespace WLFU.Entities
{
    public class AppRole: IdentityRole
    {
        public string Description { get; set; }
    }
}