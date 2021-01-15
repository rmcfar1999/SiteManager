using System;
using System.Collections.Generic;
using SiteManager.V4.Domain.Common;
#nullable disable

namespace SiteManager.V4.Domain.Entities
{
    public partial class AppLog // : AuditableEntity
    {
        public AppLog()
        {

        }

        public int AppLogId { get; set; }
        public DateTime LogDateTime { get; set; }
        public string? LogLevel { get; set; }
        public string? Category { get; set; }
        public string? EventId { get; set; }
        public string? StateInfo { get; set; }
        public string? LogMessage { get; set; }
        public string? LogException { get; set; }
        public string? UserName { get; set; }
        public string? JsonData { get; set; }


    }
}
