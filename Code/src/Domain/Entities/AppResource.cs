using System;
using System.Collections.Generic;
using SiteManager.V4.Domain.Common;
#nullable disable

namespace SiteManager.V4.Domain.Entities
{
    public partial class AppResource : AuditableEntity
    {
        public AppResource()
        {
            AppRoleResources = new HashSet<AppRoleResource>();
        }

        public int AppResourceId { get; set; }
        public string ResourceRoute { get; set; }

        public virtual ICollection<AppRoleResource> AppRoleResources { get; set; }
    }
}
