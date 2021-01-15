using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.RoleAdmin.Commands;
using SiteManager.V4.Application.UserAdmin.Queries; 
using SiteManager.V4.Application.RoleAdmin.Models;
using SiteManager.V4.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SiteManager.V4.Infrastructure.Identity;
using SiteManager.V4.Application.RoleAdmin.Queries;

namespace SiteManager.V4.Application.IntegrationTests.RoleAdmin.Commands
{
    using static Testing;

    public class UpdateRolesTests : TestBase
    {
        [Test, Order(1)]
        public async Task ShouldRequireMinimumFields()
        {
            var command = new UpdateRoleCommand();
            command.RoleName = "";

            FluentActions.Invoking(() =>
                    SendAsync(command)).Should().Throw<ValidationException>();
            
        }

        [Test, Order(2)]
        public async Task ShouldRequireUniqueRolename()
        {
            var testUser = await RunAsDefaultUserAsync();
            var roles = await SendAsync(new GetAllRolesQuery());

            var command = new UpdateRoleCommand
            {
                RoleId = roles.FirstOrDefault().AppRoleId,
                RoleName = roles.LastOrDefault().Name
            };

            FluentActions.Invoking(() =>
                  SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldUpdateRole()
        {
            var testUser = await RunAsDefaultUserAsync();

            var roles = await SendAsync(new GetAllRolesQuery()); 

            var command = new UpdateRoleCommand
            {
                RoleId = roles.FirstOrDefault().AppRoleId,
                RoleName = roles.FirstOrDefault().Name + " Updated"
            };
            var id = await SendAsync(command);

            var Role = await FindAsync<AppRole>(id);

            Role.Should().NotBeNull();
            Role.Name.Should().Be(command.RoleName);
            Role.NormalizedName.Should().Be(command.RoleName.ToUpper());

        }


    }
}