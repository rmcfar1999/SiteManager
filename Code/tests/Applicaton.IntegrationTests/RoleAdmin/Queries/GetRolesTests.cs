using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.RoleAdmin.Queries;
using SiteManager.V4.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SiteManager.V4.Infrastructure.Identity;

namespace SiteManager.V4.Application.IntegrationTests.RoleAdmin.Queries
{
    using static Testing;

    public class GetRolesTests : TestBase
    {

        [Test, Order(2)]
        public async Task ShouldReturnTestRoles()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new GetAllRolesQuery();

            var userList = await SendAsync(command);

            userList.Should().HaveCount(3); 

        }



    }
}