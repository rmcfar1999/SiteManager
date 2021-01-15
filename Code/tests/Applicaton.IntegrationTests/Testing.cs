using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Infrastructure.Identity;
using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Infrastructure.Persistence;
using SiteManager.V4.WebUI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Npgsql;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static Checkpoint _checkpoint;
    private static int _currentUserId;
    private static int _testSurveyId;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "SiteManager.V4.WebUI"));

        services.AddLogging();

        startup.ConfigureServices(services);

        IConfiguration config = startup.Configuration;
        services.AddSingleton<IConfiguration>(config); 

        // Replace service registration for ICurrentUserService
        // Remove existing registration
        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(ICurrentUserService));

        services.Remove(currentUserServiceDescriptor);

        // Register testing version
        services.AddTransient(provider =>
            Mock.Of<ICurrentUserService>(s => s.UserId == _currentUserId));

        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" }, 
            DbAdapter = DbAdapter.Postgres
        };
        

        EnsureDatabase().GetAwaiter().GetResult();
    }

    private static async Task EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Database.Migrate();

        //await ApplicationDbContextSeed.SeedSampleSurveyAsync(context);
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<IMediator>();

        return await mediator.Send(request);
    }

    public static async Task<int> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!");
    }

    public static async Task<int> RunAsUserAsync(string userName, string password)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();

        var appRole0 = new AppRole() { Name = "Public" };
        var role = await roleManager.FindByNameAsync(appRole0.Name);
        if (role == null)
        {
            var roleResult = await roleManager.CreateAsync(appRole0);
        }

        var appRole1 = new AppRole() { Name = "Test Role 1" }; 
        var role1 = await roleManager.FindByNameAsync(appRole1.Name);
        if (role == null)
        {
            var roleResult = await roleManager.CreateAsync(appRole1);
        }

        var appRole2 = new AppRole() { Name = "Test Role 2" };
        var role2 = await roleManager.FindByNameAsync(appRole2.Name);
        if (role2 == null)
        {
            var roleResult = await roleManager.CreateAsync(appRole2);
        }

        var user = await userManager.FindByEmailAsync(userName);
        if (user == null)
        {
            user = new AppUser { UserName = userName, Email = userName };
            var result = await userManager.CreateAsync(user, password);
            var addRole0 = await userManager.AddToRoleAsync(user, appRole0.Name);
            var addRole1= await userManager.AddToRoleAsync(user, appRole1.Name);
            var addRole2 = await userManager.AddToRoleAsync(user, appRole2.Name);
        }
        
        _currentUserId = user.Id;

        return _currentUserId;
    }
   
    public static async Task ResetState()
    {
        //if (!_configuration.GetConnectionString("DefaultConnection").ToLower().Contains("dev"))
        //    throw new System.Exception("Database does not appear to be a development. Cannot execute tests.");

        using (var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();

            await _checkpoint.Reset(conn);
        }
        //await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        _currentUserId = 0;
    }

    public static async Task<TEntity> FindAsync<TEntity>(int id)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(id);
    }


    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
