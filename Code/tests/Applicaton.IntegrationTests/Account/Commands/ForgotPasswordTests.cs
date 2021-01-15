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

    public class ForgotPasswordTests : TestBase
    {
        private readonly string _passwordResetUrl = "https://localhost:44312/identity/account/resetpassword";

        [Test, Order(1)]
        public async Task ShouldRequireMinimumFields()
        {
            var userId = await RunAsDefaultUserAsync();
            var command = new ForgotPasswordCommand();
            command.Email = "";

            FluentActions.Invoking(() =>
                    SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test, Order(2)]
        public async Task ShouldRequireValidEmail()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new ForgotPasswordCommand
            {
                Email = "newEmailnunit", 
                PasswordResetUrl = _passwordResetUrl
            };

            FluentActions.Invoking(() =>
                   SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test, Order(2)]
        public async Task ShouldProcessResetPasswordRequest()
        {
            var userId = await RunAsDefaultUserAsync();
            var user = await FindAsync<AppUser>(userId);

            var command = new ForgotPasswordCommand
            {
                Email = user.Email,
                PasswordResetUrl = _passwordResetUrl
            };
            var id = await SendAsync(command);

            user = await FindAsync<AppUser>(id);

            //nothing to validate here as it's email based.
            user.Should().NotBeNull();
            user.Email.Should().Be(command.Email);
            user.NormalizedEmail.Should().Be(command.Email.ToUpper());
            

        }


    }
}