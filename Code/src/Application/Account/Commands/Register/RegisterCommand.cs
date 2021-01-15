using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SiteManager.V4.Application.Common.Exceptions;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SiteManager.V4.Application.Account.Commands
{
    public partial class RegisterCommand : IRequest<int>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ReturnUrl { get; set; }
        public string ConfirmationUrl { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<RegisterCommand> _logger; 

        public RegisterCommandHandler(IIdentityService identityService, ILogger<RegisterCommand> logger)
        {
            _identityService = identityService;
            _logger = logger; 
        }

        public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var r = await _identityService.CreateUserAsync(request.UserName, request.Email, request.PhoneNumber, request.Password);
            if (!r.Result.Succeeded)
            {
                throw new ValidationException("User", r.Result.Errors);
            }

            var user = await _identityService.GetUserAsync(r.UserId);
            user.PhoneNumber = request.PhoneNumber;
            user.UserName = request.UserName;
            user.Email = request.Email;
            r = await _identityService.UpdateUser(user.AppUserId, user.UserName, user.Email, user.PhoneNumber);

            if (r.Result.Succeeded)
            { 
                _logger.LogInformation("User registered a new account with password.");

                //todo what are the default roles for public registration
                foreach (var role in new List<string>() { "Public" }) 
                {
                    r = await _identityService.RemoveUserRole(user.AppUserId, role);
                    r = await _identityService.AddUserRole(user.AppUserId, role);
                }

                await _identityService.SendRegistrationConfirmation(user.AppUserId, request.ConfirmationUrl);
            }

            return r.UserId;


        }
    }
}
