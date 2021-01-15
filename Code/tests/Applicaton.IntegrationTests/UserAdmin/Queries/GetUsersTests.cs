using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.UserAdmin.Queries;
using SiteManager.V4.Application.UserAdmin.Models;
using SiteManager.V4.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SiteManager.V4.Infrastructure.Identity;

namespace SiteManager.V4.Application.IntegrationTests.UserAdmin.Queries
{
    using static Testing;

    public class GetUsersTests : TestBase
    {

        [Test, Order(2)]
        public async Task ShouldReturnTestUser()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new GetAllUsersQuery();

            var userList = await SendAsync(command);

            userList.Should().HaveCount(1);
            userList.First().AppRoles.Should().HaveCount(3);

        }



    }
}