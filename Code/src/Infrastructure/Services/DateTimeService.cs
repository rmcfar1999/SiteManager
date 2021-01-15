using SiteManager.V4.Application.Common.Interfaces;
using System;

namespace SiteManager.V4.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
