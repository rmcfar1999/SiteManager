using System;

namespace SiteManager.V4.Domain.Common
{
    public abstract class AuditableEntity
    {
        public int CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
