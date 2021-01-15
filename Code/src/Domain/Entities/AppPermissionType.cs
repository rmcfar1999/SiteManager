using System;
using System.Collections.Generic;
using SiteManager.V4.Domain.Common;
#nullable disable

namespace SiteManager.V4.Domain.Entities
{
    public partial class AppPermissionType : AuditableEntity
    {
        public AppPermissionType()
        {
            this.AppRoleResources = new HashSet<AppRoleResource>();
            
        }

        public int AppPermissionTypeId { get; set; }
        public string PermissionType { get; set; }

        public virtual ICollection<AppRoleResource> AppRoleResources { get; set; }
       
    }
}
