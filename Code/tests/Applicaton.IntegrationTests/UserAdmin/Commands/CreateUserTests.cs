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
using SiteManager.V4.Application.UserAdmin.Queries;

namespace SiteManager.V4.Application.IntegrationTests.UserAdmin.Commands
{
    using static Testing;

    public class CreateUsersTests : TestBase
    {
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

        [Test, Order(2)]
        public async Task ShouldRequireValidEmail()
        {
            var userId = await RunAsDefaultUserAsync();

            var testUser = await SendAsync(new GetAllUsersQuery());
            var testRoles = testUser.FirstOrDefault().AppRoles.ToList();

            var command = new CreateUserCommand
            {
                Email = "newEmailnunit",
                UserName = "NewUserName",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1",
                PhoneNumber = "8015551212",
                AppRoles = testRoles
            };
            FluentActions.Invoking(() =>
                   SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldRequireUniqueUsername()
        {
            var userId = await RunAsDefaultUserAsync();

            var testUser = await SendAsync(new GetAllUsersQuery());
            var testRoles = testUser.FirstOrDefault().AppRoles.ToList();

            var command = new CreateUserCommand
            {
                Email = "newEmail@nunit.com",
                UserName = "test@local",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1",
                PhoneNumber = "8015551212",
                AppRoles = testRoles
            };
            FluentActions.Invoking(() =>
                  SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldRequireUniqueEmail()
        {
            var userId = await RunAsDefaultUserAsync();
            var testUser = await SendAsync(new GetAllUsersQuery());
            var testRoles = testUser.FirstOrDefault().AppRoles.ToList();
            var command = new CreateUserCommand
            {
                Email = "test@local",
                UserName = "test123@local",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1",
                PhoneNumber = "8015551212",
                AppRoles = testRoles
            };
            FluentActions.Invoking(() =>
                  SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldRequireValidRoles()
        {
            var userId = await RunAsDefaultUserAsync();

            var testUser = await SendAsync(new GetAllUsersQuery());
            var testRoles = testUser.FirstOrDefault().AppRoles.ToList();

            testRoles.Add("Bad Role");
            var command = new CreateUserCommand
            {
                Email = "newEmail@nunit.com",
                UserName = "NewUserName",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1",
                PhoneNumber = "8015551212",
                AppRoles = testRoles

            };

            FluentActions.Invoking(() =>
                  SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldRequireRoles()
        {
            var userId = await RunAsDefaultUserAsync();

            var testRoles = new List<string>(); //roles required

            testRoles.Add("Bad Role");
            var command = new CreateUserCommand
            {
                Email = "newEmail@nunit.com",
                UserName = "NewUserName",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1",
                PhoneNumber = "8015551212",
                AppRoles = testRoles

            };

            FluentActions.Invoking(() =>
                  SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldCreateUserWithRoles()
        {
            var userId = await RunAsDefaultUserAsync();

            var testUser = await SendAsync(new GetAllUsersQuery());
            var testRoles = testUser.FirstOrDefault().AppRoles.ToList();
            var command = new CreateUserCommand
            {
                Email = "newEmail@nunit.com",
                UserName = "NewUserName",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1",
                PhoneNumber = "8015551212",
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