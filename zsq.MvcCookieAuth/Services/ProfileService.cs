using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using zsq.MvcCookieAuth.Models;

namespace zsq.MvcCookieAuth.Services
{
    public class ProfileService : IProfileService
    {
        private UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private async Task<List<Claim>> GetClaimFormUserAsync(ApplicationUser user)
        {
            var claims = new List<Claim>{
                new Claim(JwtClaimTypes.Subject,user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName,user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            if (!string.IsNullOrEmpty(user.Avatar))
            {
                claims.Add(new Claim("avatar", user.Avatar));
            }

            return claims;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            var claims = await GetClaimFormUserAsync(user);
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = false;

            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            //Todo：还需要判断user是不是被lock了，才能设置IsActive=true
            context.IsActive = user != null;
        }
    }
}