using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiteManager.V4.Infrastructure.Identity;
using SiteManager.V4.Infrastructure.Persistence;

[assembly: HostingStartup(typeof(SiteManager.V4.WebUI.Areas.Identity.IdentityHostingStartup))]
namespace SiteManager.V4.WebUI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}