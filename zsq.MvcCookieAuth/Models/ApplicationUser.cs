using Microsoft.AspNetCore.Identity;

namespace zsq.MvcCookieAuth.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Avatar { get; set; }
    }
}