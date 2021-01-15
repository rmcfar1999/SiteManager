using System;
using System.Collections.Generic;
using SiteManager.V4.Domain.Common;
#nullable disable

namespace SiteManager.V4.Domain.Entities
{
    public partial class AppRoleResource : AuditableEntity
    {
        public AppRoleResource()
        {
            
        }

        public int AppRoleResourceId { get; set; }
        public int AppRoleId { get; set; }
        public int AppResourceId { get; set; }
        public int AppPermissionTypeId { get; set; }

        public virtual AppPermissionType AppPermissionType { get; set; }
        public virtual AppResource AppResource { get; set; }
    }
}
