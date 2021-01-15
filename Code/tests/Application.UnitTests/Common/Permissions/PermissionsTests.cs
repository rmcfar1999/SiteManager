using SiteManager.V4.Application.Common.Mappings;
using SiteManager.V4.Application.TodoLists.Queries.GetTodos;
using NUnit.Framework;
using SiteManager.V4.Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using SiteManager.V4.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SiteManager.V4.Infrastructure.Services;
using System.Security.Claims;

namespace SiteManager.V4.Application.UnitTests.Common.Mappings
{
    public class PermissionsTests
    {

        public PermissionsTests()
        {
        }

        [Test]
        public async Task RoleAuthorizationHandler_Should_Succeed()
        {
            //Arrange    
            var requirements = new[] { new RoleRequirement("Public") };
            var user = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[] {
                                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Public", "String", "https://localhost:44312"),
                                },
                            "Basic")
                        );
            var resource = new AppResource
            {
                ResourceRoute = "/admin/roleadmin",
            };
            var context = new AuthorizationHandlerContext(requirements, user, resource);
            var subject = new RoleRequirementHandler();

            //Act
            await subject.HandleAsync(context);

            //Assert
            context.HasSucceeded.Should().BeTrue(); //FluentAssertions
        }

        [Test]
        public async Task RoleAuthorizationHandler_Multi_Should_Succeed()
        {
            //Arrange    
            var requirements = new[] { new RoleRequirement("Public,Test Role 1") };
            var user = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[] {
                                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Test Role 1", "String", "https://localhost:44312"),
                                },
                            "Basic")
                        );
            var resource = new AppResource
            {
                ResourceRoute = "/admin/roleadmin",
            };
            var context = new AuthorizationHandlerContext(requirements, user, resource);
            var subject = new RoleRequirementHandler();

            //Act
            await subject.HandleAsync(context);

            //Assert
            context.HasSucceeded.Should().BeTrue(); //FluentAssertions
        }

        [Test]
        public async Task RoleAuthorizationHandler_Multi_Should_Fail()
        {
            //Arrange    
            var requirements = new[] { new RoleRequirement("Public,Test Role 1") };
            var user = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[] {
                                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Bad User Role", "String", "https://localhost:44312"),
                                },
                            "Basic")
                        );
            var resource = new AppResource
            {
                ResourceRoute = "/admin/roleadmin",
            };
            var context = new AuthorizationHandlerContext(requirements, user, resource);
            var subject = new RoleRequirementHandler();

            //Act
            await subject.HandleAsync(context);

            //Assert
            context.HasSucceeded.Should().BeFalse();
        }

        //[Test]
        //public async Task PermissionAuthorizationHandler_Should_Succeed()
        //{
        //    //Arrange    
        //    var requirements = new[] { new RoleRequirement("Public,Test Role 1") };
        //    var user = new ClaimsPrincipal(
        //                new ClaimsIdentity(
        //                    new Claim[] {
        //                        new Claim(ClaimsIdentity.DefaultRoleClaimType, "Test Role 1", "String", "https://localhost:44312"),
        //                        },
        //                    "Basic")
        //                );
        //    var resource = new AppResource
        //    {
        //        ResourceRoute = "/admin/roleadmin",
        //    };
        //    var context = new AuthorizationHandlerContext(requirements, user, resource);
        //    var subject = new PermissionRequirementHandler(permSvc, identSvc, logger);

        //    //Act
        //    await subject.HandleAsync(context);

        //    //Assert
        //    context.HasSucceeded.Should().BeTrue(); //FluentAssertions
        //}
    }
}
