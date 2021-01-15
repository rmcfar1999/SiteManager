using System;
using System.Collections.Generic;
using System.Text;
using SiteManager.V4.Application.Common.Models;

namespace SiteManager.V4.Application.RoleAdmin.Models
{
    public class AppRoleVm
    {
        public AppRoleVm()
        {
            AppUsers = new List<AppRoleDto>();  
        }

        public IList<AppRoleDto> AppUsers { get; set; }
    }
}
