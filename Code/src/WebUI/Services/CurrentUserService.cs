using SiteManager.V4.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SiteManager.V4.WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            int _userId = 0; 
            var _identityId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(_identityId, out _userId))
                UserId = _userId;
            else
                UserId = 0; // throw new System.Exception("User ClaimType Name Identifier Not an Integer"); 

            //UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public int UserId { get; }
    }
}
