using System;
using System.Collections.Generic;
using System.Text;
using SiteManager.V4.Application.Common.Models;

namespace SiteManager.V4.Application.UserAdmin.Models
{
    public class AppUserVm
    {
        public AppUserVm()
        {
            AppUsers = new List<AppUserDto>();  
        }

        public IList<AppUserDto> AppUsers { get; set; }
    }
}
