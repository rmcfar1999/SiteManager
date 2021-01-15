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

    public class DeleteRolesTests : TestBase
    {
        [Test, Order(2)]
        public async Task ShouldDeleteRole()
        {
            var user = await RunAsDefaultUserAsync();

            var query = new GetAllRolesQuery();

            var roleList = await SendAsync(query);

            var command = new DeleteRoleCommand
            {
                RoleId = roleList.First().AppRoleId
            };

            var r = await SendAsync(command);

            var Role = await FindAsync<AppRole>(command.RoleId);

            Role.Should().BeNull();


        }



    }
}

