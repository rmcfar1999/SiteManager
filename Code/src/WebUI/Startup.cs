using SiteManager.V4.Application;
using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Infrastructure;
using SiteManager.V4.Infrastructure.Persistence;
using SiteManager.V4.Infrastructure.Services;
using SiteManager.V4.WebUI.Filters;
using SiteManager.V4.WebUI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Linq;
using SiteManager.V4.Application.Common.Behaviours;
using MediatR;

namespace SiteManager.V4.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            //Hook up a cqrs logging pipeline for all requests/responses from app layer
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            //Add CORS
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("Open", builder =>
            //    {
            //        builder.AllowAnyOrigin();
            //        builder.AllowAnyHeader();
            //        builder.AllowAnyMethod();
            //    });
            //});
            services.AddScoped<ApiExceptionFilter>();
            services.AddControllersWithViews(options => 
                options.Filters.Add(new ApiExceptionFilter()));

            services.AddRazorPages();

            
            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "SiteManager.V4 API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            ILoggerFactory log, 
            IApplicationDbContext logDbContext, 
            ICurrentUserService currentUserService,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IDateTime dateTime)

        {
            //See infrastructure DependencyInjection.cs
            log.AddProvider(new BasicSQLLogProvider(Configuration,currentUserService, operationalStoreOptions,dateTime)); 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });

            app.UseRouting();

            app.UseAuthentication();
            
            //enable the identity server middleware aka the "connect/xxx" urls
            //https://localhost:44312/.well-known/openid-configuration
            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //Dev environment is setup to depend on/proxy VSCode/Node to serve the angular
                    //SPA app.  If wanting to run entirely from visual studio:
                    //Uncomment the following two lines AND comment the below UseProxyToSpa line.

                    //spa.Options.StartupTimeout = new System.TimeSpan(0, 5, 0); 
                    //spa.UseAngularCliServer(npmScript: "start");

                    //RMC: the middleware in 3.1 is funky and seems to randomly go nuts 
                    //waiting for the CLI server "listening on" text..going to try use a manual serve/start
                    //also its is slower on angular live/debug updates and reloading.

                    //so....start node angular (ng serve) first and then run this project.
                    //below will create a proxy to the default node/angular server.
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    
                }
            });
        }
    }
}
