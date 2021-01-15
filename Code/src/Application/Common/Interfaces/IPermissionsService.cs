using System.Collections.Generic;
using SiteManager.V4.Domain.Entities;

namespace SiteManager.V4.Application.Common.Interfaces
{
    public interface IPermissionsService
    {
        bool HasPermission(List<int> userRoles, string resource, string permissionType);

        List<AppRoleResource> Permissions { get; set; }
    }
}