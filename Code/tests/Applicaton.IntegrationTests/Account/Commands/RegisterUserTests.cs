using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.Account.Commands;
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


namespace SiteManager.V4.Application.IntegrationTests.Account.Commands
{
    using static Testing;

    public class CreateUsersTests : TestBase
    {
        private readonly string _emailConfirmUrl = "https://localhost:44312/identity/account/confirmemail";

        [Test, Order(1)]
        public async Task ShouldRequireMinimumFields()
        {
            var userId = await RunAsDefaultUserAsync();
            var command = new RegisterCommand();
            command.UserName = "";
            command.Email = "";

            FluentActions.Invoking(() =>
                    SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test, Order(2)]
        public async Task ShouldRequireValidEmail()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new RegisterCommand
            {
                Email = "newEmailnunit",
                UserName = "NewUserName",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1"                
            };

            FluentActions.Invoking(() =>
                   SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldRequireUniqueUsername()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new RegisterCommand
            {
                Email = "newEmail@nunit.com",
                UserName = "test@local",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1",
                PhoneNumber = "8015551212"
            };
            FluentActions.Invoking(() =>
                  SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldRequireUniqueEmail()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new RegisterCommand
            {
                Email = "test@local",
                UserName = "test123@local",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1",
                PhoneNumber = "8015551212"
            };
            FluentActions.Invoking(() =>
                  SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldRegisterUser()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new RegisterCommand
            {
                Email = "newEmail@nunit.com",
                UserName = "NewUserName",
                Password = "Password%$1",
                ConfirmPassword = "Password%$1",
                PhoneNumber = "8015551212",
                ConfirmationUrl = _emailConfirmUrl

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