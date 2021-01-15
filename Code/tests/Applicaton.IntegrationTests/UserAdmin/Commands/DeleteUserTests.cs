using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.UserAdmin.Commands;
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

namespace SiteManager.V4.Application.IntegrationTests.UserAdmin.Commands
{
    using static Testing;

    public class DeleteUsersTests : TestBase
    {
        [Test, Order(2)]
        public async Task ShouldDeleteUser()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new DeleteUserCommand
            {
                UserId = userId
            };

            var r = await SendAsync(command);

            var user = await FindAsync<AppUser>(userId);

            user.Should().BeNull();


        }



    }
}

