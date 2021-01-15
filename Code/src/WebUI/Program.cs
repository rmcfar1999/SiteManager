using SiteManager.V4.Infrastructure.Identity;
using SiteManager.V4.Infrastructure.Persistence;
using SiteManager.V4.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SiteManager.V4.WebUI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                   // if (context.Database.IsSqlServer())
                   // {
                        context.Database.Migrate();
                   // }                   

                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

                    await ApplicationDbContextSeed.SeedDefaultRoleAsync(roleManager);
                    await ApplicationDbContextSeed.SeedAdminUserAsync(userManager);
                    await ApplicationDbContextSeed.SeedDemoUserAsync(userManager);
                    await ApplicationDbContextSeed.SeedDefaultPermissions(context, roleManager);
                    await ApplicationDbContextSeed.SeedSampleDataAsync(context);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        var env = context.HostingEnvironment;
                        if (!env.IsDevelopment())
                        {
                            webBuilder.UseKestrel(opts =>
                            {
                                var configuration = opts.ApplicationServices.GetService<IConfiguration>();
                                opts.Listen(IPAddress.Loopback, 5000);
                                var env = opts.ApplicationServices.GetService<IWebHostEnvironment>();
                                opts.Listen(IPAddress.Loopback, 5001, listenOptions =>
                                {
                                    listenOptions.UseHttps(
                                        new X509Certificate2(configuration["SSLCertificate:FilePath"], configuration["SSLCertificate:Password"])
                                    );
                                });
                            });
                        }
                    });
                   
                });
    }
}
