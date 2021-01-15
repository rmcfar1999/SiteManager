using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.UserAdmin.Commands;
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

namespace SiteManager.V4.Application.IntegrationTests.UserAdmin.Commands
{
    using static Testing;

    public class UpdateUsersTests : TestBase
    {
        private readonly string _passwordResetUrl = "https://localhost:44312/identity/account/resetpassword";

        [Test, Order(1)]
        public async Task ShouldRequireMinimumFields()
        {
            var userId = await RunAsDefaultUserAsync();
            var command = new UpdateUserCommand();
            command.UserName = "";
            command.Email = "";

            FluentActions.Invoking(() =>
                    SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test, Order(1)]
        public async Task ShouldRequireValidRoles()
        {
            var userId = await RunAsDefaultUserAsync();

            var testUser = await SendAsync(new GetAllUsersQuery());
            var testRoles = testUser.FirstOrDefault().AppRoles.ToList();
            testRoles.Add("Bad Role Test");

            var command = new UpdateUserCommand
            {
                AppUserId = userId,
                Email = "updatedEmail@update.com",
                UserName = "updatedUserName",
                PhoneNumber = "8015551212",
                ResetPassword = true,
                AppRoles = testRoles

            };

            FluentActions.Invoking(() =>
                    SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test, Order(1)]
        public async Task ShouldRequireRoles()
        {
            var userId = await RunAsDefaultUserAsync();

            var testRoles = new List<string>();

            var command = new UpdateUserCommand
            {
                AppUserId = userId,
                Email = "updatedEmail@update.com",
                UserName = "updatedUserName",
                PhoneNumber = "8015551212",
                ResetPassword = true,
                AppRoles = testRoles

            };

            FluentActions.Invoking(() =>
                    SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldUpdateUserWithPasswordReset()
        {
            var userId = await RunAsDefaultUserAsync();

            var testUser = await SendAsync(new GetAllUsersQuery());
            var testRoles = testUser.FirstOrDefault().AppRoles.ToList();

            var command = new UpdateUserCommand
            {
                AppUserId = userId,
                Email = "updatedEmail@update.com",
                UserName = "updatedUserName",
                PhoneNumber = "8015551212",
                ResetPassword = true,
                AppRoles = testRoles, 
                PasswordResetUrl = _passwordResetUrl

            };
            var id = await SendAsync(command);

            var user = await FindAsync<AppUser>(id);

            user.Should().NotBeNull();
            user.Email.Should().Be(command.Email);
            user.NormalizedEmail.Should().Be(command.Email.ToUpper());
            user.UserName.Should().Be(command.UserName);
            user.NormalizedUserName.Should().Be(command.UserName.ToUpper());
            user.PhoneNumber.Should().Be(command.PhoneNumber);

        }


        [Test, Order(2)]
        public async Task ShouldUpdateUser()
        {
            var userId = await RunAsDefaultUserAsync();

            var testUser = await SendAsync(new GetAllUsersQuery());
            var testRoles = testUser.FirstOrDefault().AppRoles.ToList();

            var command = new UpdateUserCommand
            {
                AppUserId = userId,
                Email = "test@local",
                UserName = "test@local",
                PhoneNumber = "8015551212",
                ResetPassword = false,
                AppRoles = testRoles
            };
            var id = await SendAsync(command);

            var user = await FindAsync<AppUser>(id);

            user.Should().NotBeNull();
            user.Email.Should().Be(command.Email);
            user.NormalizedEmail.Should().Be(command.Email.ToUpper());
            user.UserName.Should().Be(command.UserName);
            user.NormalizedUserName.Should().Be(command.UserName.ToUpper());
            user.PhoneNumber.Should().Be(command.PhoneNumber);

        }


    }
}