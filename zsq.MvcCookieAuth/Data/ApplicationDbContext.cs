using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using zsq.MvcCookieAuth.Models;

namespace zsq.MvcCookieAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}