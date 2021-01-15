using SiteManager.V4.Application.Common.Mappings;
using SiteManager.V4.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteManager.V4.Application.Common.Models
{
    public class AppUserDto 
    {
        public AppUserDto() {
            //AppRoles = new List<AppRoleDto>(); 
            AppRoles = new List<string>(); 
        }

        public int AppUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }

        //public IList<AppRoleDto> AppRoles { get; set; }
        public IList<string> AppRoles { get; set; }
    }
}
