using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using zsq.MvcCookieAuth.Models;

namespace zsq.MvcCookieAuth.Data
{
    public class ApplicationDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationUserRole> _roleManager;

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
        {
            if (!context.Roles.Any())
            {
                _roleManager = services.GetRequiredService<RoleManager<ApplicationUserRole>>();

                var role = new ApplicationUserRole
                {
                    Name = "Administrator",
                    NormalizedName = "Administrator"
                };
                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    throw new Exception("初始默认角色失败" + result.Errors.SelectMany(e => e.Description));
                }
            }

            if (!context.Users.Any())
            {
                _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                var defaultUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "18576645357@163.com",
                    NormalizedUserName = "admin",
                    Avatar = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1526955882826&di=d53ce0d491bf61a180194fd358641f81&imgtype=0&src=http%3A%2F%2Fimg.mukewang.com%2F5a77b61000013ca502560192.jpg"
                };

                var result = await _userManager.CreateAsync(defaultUser, "123456");
                await _userManager.AddToRoleAsync(defaultUser, "Administrator");

                if (!result.Succeeded)
                {
                    throw new Exception("初始默认用户失败");
                }
            }
        }
    }
}