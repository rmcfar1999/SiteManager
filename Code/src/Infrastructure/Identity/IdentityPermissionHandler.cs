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
using System;

namespace SiteManager.V4.Infrastructure.Identity
{
    #region auth handlers...todo: move these?...not really infrastructure? maybe?  
    public class PermissionRequirementHandler : IAuthorizationHandler
    {
        private readonly IPermissionsService _permissions;
        private readonly IIdentityService _identityService;
        private readonly ILogger<PermissionRequirementHandler> _logger;

        //const string currentAuthority = "https://sitemanager.socialslice.net";// "https://localhost:44312"; //todo..this needs to be dynamic/option'd
        public PermissionRequirementHandler(IPermissionsService permissions, IIdentityService identityService, ILogger<PermissionRequirementHandler> logger)
        {
            _permissions = permissions;
            _identityService = identityService;
            _logger = logger;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var requirement in pendingRequirements)
            {
                if (requirement is AppPermissionRequirement)
                {
                    var userRoleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value.ToLower()); //validate issuer?//&& c.Issuer.ToLower() == currentAuthority).Select(x => x.Value.ToLower());
                    //_logger.LogWarning("Permission handler is using a hardcoded claim issuer!!");
                    //ToDo: cache the rolls https://www.c-sharpcorner.com/article/asp-net-core-in-memory-caching/
                    var userRoles = await _identityService.GetAllRolesAsync();
                    var userRoleIds = userRoles.Where(x => userRoleClaims.Contains(x.Name.ToLower())).Select(x => x.AppRoleId).ToList();

                    //ToDo: other platforms will/could have different endpoint/resource info? 
                    //make this possibly a parameter passed or check linux platforms
                    if (_permissions.HasPermission(
                        userRoleIds,
                        ((Microsoft.AspNetCore.Routing.RouteEndpoint)context.Resource).RoutePattern.RawText.ToString(),
                        ((AppPermissionRequirement)requirement).AppPermission))
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            //return Task.CompletedTask;
        }
    }

    public class AppPermissionRequirement : IAuthorizationRequirement
    {
        public AppPermissionRequirement(string permType)
        {
            AppPermission = permType;
        }

        public string AppPermission { get; set; }

    }

    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        
        //const string currentAuthority = "https://localhost:44312"; //todo..this needs to be dynamic/option'd

        public RoleRequirementHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (requirement is RoleRequirement)
            {
                //var userName = requirement.UserName;
                var pendingRequirements = context.PendingRequirements.ToList();
                if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))//&& c.Issuer.ToLower() == currentAuthority))
                {
                    //No roles in claims so fail/complete without setting success
                    return Task.CompletedTask;
                }

                //if in role
                var userRoleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value.ToLower()); //validate issuer?  && c.Issuer.ToLower() == currentAuthority).Select(x => x.Value);
                if (requirement.RequiredRoles.Select(x=>x.ToLower()).Intersect(userRoleClaims).Any())
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;

        }
    }

    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(string requiredRole)
        {
            RequiredRoles = requiredRole.Split(',').Select(x => x.Trim()).ToArray();
        }

        public string[] RequiredRoles { get; }
    }
    #endregion
}
