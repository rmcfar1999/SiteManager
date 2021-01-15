using SiteManager.V4.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.Services;
using IdentityServer4.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using SiteManager.V4.Application.Common.Interfaces;

namespace SiteManager.V4.Infrastructure.Identity
{
    public class IdentityProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public IdentityProfileService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            IList<string> roles = await _userManager.GetRolesAsync(user);

            IList<Claim> roleClaims = new List<Claim>();
            roleClaims.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            foreach (string role in roles)
            {
                roleClaims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            context.IssuedClaims.AddRange(roleClaims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }

   

}