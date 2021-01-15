using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Infrastructure.Files;
using SiteManager.V4.Infrastructure.Identity;
using SiteManager.V4.Infrastructure.Persistence;
using SiteManager.V4.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SiteManager.V4.Infrastructure
{
    
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("SiteManager.V4Db"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))); 
                //options.UseSqlServer(
                //   configuration.GetConnectionString("DefaultConnection"),
                //   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));;
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders(); // removes all providers from LoggerFactory
                logBuilder.AddConfiguration(configuration.GetSection("Logging").GetSection("BasicSQLLogProvider"));
                logBuilder.AddConsole();
                //logBuilder.AddTraceSource("Information, ActivityTracing"); // Add Trace listener provider
            });


            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailSender, EmailService>(); //For identity todo: work on consolidating these email services
            services.AddScoped<IPermissionsService, PermissionsService>();  //ToDo: fix this up as a singleton?  inject the system/db perms to avoid dependency on dbcontext and complexity? 

            #region Identity security policies for authorization rules
            //todo: move this to a separate di extension if we continue this model and it grows to permissions 
            //level/detail.
            services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, RoleRequirementHandler>();
            #endregion

            services.AddDefaultIdentity<AppUser>(options => options.User.RequireUniqueEmail = true)
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //setup the primary ids4 server and add authorization pointed at our custom identity models/context
            //https://identityserver4.readthedocs.io/en/release/reference/options.html#userinteraction
            services.AddIdentityServer(options =>
            {
                //options.PublicOrigin = "https://oauth.test.example.com"; //can overwrite the urls in discovery document.
                options.UserInteraction.LoginUrl = "/authentication/userlogin";
            })
            .AddApiAuthorization<AppUser, ApplicationDbContext>(options => { var i = options.ApiResources;});
            //RMC: temp leaving here...testing out using direct file type in appsettings:identityserver:key 
            //.LoadSigningCredentialFrom(configuration["certificates:signing"]);

            //todo: db lookup/ioptions for permtypes
            List<string> permTypes = new List<string>() { "Create", "Read", "Update", "Delete" };
            //Configure/add base authorization policy
            services.AddAuthorization(options => {
                foreach (var s in permTypes)
                {
                    options.AddPolicy(s, policy => {
                        policy.AddRequirements(new AppPermissionRequirement(s));
                    });
                }
                options.AddPolicy("IsAdministrator", policy =>
                    policy.Requirements
                        .Add(new RoleRequirement("Administrators"))
                    );
                options.AddPolicy("IsPublic", policy =>
                    policy.Requirements
                        .Add(new RoleRequirement("Public"))
                    );
            });

            //turn on/configure authentication and setup the defaults for jwt/bearer header authentication
            var authBuilder = services.AddAuthentication(options => {})
                .AddIdentityServerJwt();
                //.AddJwtBearer(options => { });

            //Identity profile scope (including roles/claims)
            services.AddTransient<IProfileService, IdentityProfileService>(); 

            ///test code below...change cookie security level
            //services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            //    })
            //    .AddIdentityServerJwt();

            return services;
        }
    }

    //public static class IdentityServerBuilderExtensions
    //{
    //    public static IIdentityServerBuilder LoadSigningCredentialFrom(this IIdentityServerBuilder builder, string path)
    //    {
    //        if (!string.IsNullOrWhiteSpace(path))
    //        {
    //            builder.AddSigningCredential(new X509Certificate2(path, "password"));
    //        }
    //        else
    //        {
    //            builder.AddDeveloperSigningCredential();
    //        }

    //        return builder;
    //    }
    //}
}
