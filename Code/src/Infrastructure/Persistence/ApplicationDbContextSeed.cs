using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteManager.V4.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDemoUserAsync(UserManager<AppUser> userManager)
        {
            var defaultUser = new AppUser { UserName = "SiteManDemo", Email = "sitemandemo@socialslice.com" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                var result = await userManager.CreateAsync(defaultUser, "DemoPassword!123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Administrators");
                    await userManager.AddToRoleAsync(defaultUser, "Public");
                }
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            var defaultUser = new AppUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                var result = await userManager.CreateAsync(defaultUser, "DevPass2020!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Administrators");
                    await userManager.AddToRoleAsync(defaultUser, "Public");
                }
            }
        }

        public static async Task SeedDefaultRoleAsync(RoleManager<AppRole> roleManager)
        {
            foreach (var r in new List<string>() { "Administrators", "Public" })
            {
                var role = new AppRole { Name = r };

                if (roleManager.Roles.All(u => u.NormalizedName != role.Name.ToUpper()))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        public static async Task SeedDefaultPermissions(ApplicationDbContext context, RoleManager<AppRole> roleManager)
        {
            IEnumerable<AppPermissionType> permissionTypes = context.AppPermissionType.AsEnumerable();
            IEnumerable<AppRoleResource> roleResources = context.AppRoleResource.AsEnumerable();
            IEnumerable<AppResource> appResources = context.AppResource.AsEnumerable();
            var adminRole = await roleManager.FindByNameAsync("Administrators"); 

            if (permissionTypes.Any() || appResources.Any() || roleResources.Any())
                return;

            List<string> defaultPermissionTypes = new List<string>() { "Create", "Read", "Update", "Delete" };
            List<string> defaultResources = new List<string>() { "api/RoleAdmin", "api/UserAdmin", "api/UserAdmin/{id}", "api/Roleadmin/{id}" };
            
            //create default crud permission types 
            foreach (var t in defaultPermissionTypes)
            {
                context.AppPermissionType.Add(new AppPermissionType() { PermissionType = t });
            }
            context.SaveChanges();
            permissionTypes = context.AppPermissionType.ToList(); // AsEnumerable();

            //create the basic role/user admin resources
            foreach (var resource in defaultResources)
            {
                context.AppResource.Add(new AppResource()
                {
                    ResourceRoute = resource
                });
            }
            context.SaveChanges();
            appResources = context.AppResource.ToList();

            //Add the links to resource/perm and role s
            foreach (var permType in permissionTypes)
            {
                foreach (var resource in appResources)
                {
                    context.AppRoleResource.Add(new AppRoleResource()
                    {
                        AppResourceId = resource.AppResourceId,
                        AppPermissionTypeId = permType.AppPermissionTypeId,
                        AppRoleId = adminRole.Id
                    });
                    context.SaveChanges();
                }
            }
           // context.SaveChanges();
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.TodoLists.Any())
            {
                context.TodoLists.Add(new TodoList
                {
                    Title = "Shopping",
                    Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" },
                        new TodoItem { Title = "Water" }
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
