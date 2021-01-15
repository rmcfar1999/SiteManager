using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.RoleAdmin.Commands;
using SiteManager.V4.Application.RoleAdmin.Queries;
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

namespace SiteManager.V4.Application.IntegrationTests.RoleAdmin.Commands
{
    using static Testing;

    public class CreateRolesTests : TestBase
    {
        [Test, Order(1)]
        public async Task ShouldRequireMinimumFields()
        {
            var RoleId = await RunAsDefaultUserAsync();
            var command = new UpdateRoleCommand();
            command.RoleName = "";
            

            FluentActions.Invoking(() =>
                    SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test, Order(2)]
        public async Task ShouldRequireUniqueRolename()
        {
            var RoleId = await RunAsDefaultUserAsync();
            var roles = await SendAsync(new GetAllRolesQuery());

            var command = new CreateRoleCommand
            {
                RoleName = roles.LastOrDefault().Name
            };
            FluentActions.Invoking(() =>
                  SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldCreateRole()
        {
            var RoleId = await RunAsDefaultUserAsync();

            var command = new CreateRoleCommand
            {
                RoleName = "NewRoleName"
            };
            var id = await SendAsync(command);

            var Role = await FindAsync<AppRole>(id);

            Role.Should().NotBeNull();
            Role.Name.Should().Be(command.RoleName);
            Role.NormalizedName.Should().Be(command.RoleName.ToUpper());

        }


    }
}