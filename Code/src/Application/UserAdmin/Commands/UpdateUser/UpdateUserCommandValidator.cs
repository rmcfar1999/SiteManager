using SiteManager.V4.Application.Common.Interfaces;
using FluentValidation;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace SiteManager.V4.Application.UserAdmin.Commands
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdateUserCommandValidator(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;

            RuleFor(v => v.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(256).WithMessage("User name must be less than 256 characters.")
                .Must((o, UpdateUserCommand) => { return BeUniqueUserName(o.AppUserId, o.UserName).Result; })
                .WithMessage("This username is taken by another user.");

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MaximumLength(256).WithMessage("Email must be less than 256 characters.")
                .Must((o, UpdateUserCommand) => { return BeUniqueEmail(o.AppUserId, o.Email).Result; })
                .WithMessage("This email is taken by another user.");

            RuleFor(v => v.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(12).WithMessage("Phone number must be less than 256 characters.");

            RuleFor(v => v.AppRoles)
                .NotEmpty().WithMessage("Roles are required for users.  Use 'Public' if unsure.")
                .MustAsync(RolesExists)
                .WithMessage("A role specified was not found in the system.");
        }

        public async Task<bool> BeUniqueUserName(int userId, string userName)
        {
            var user = await _identityService.GetUserAsync(userName);
            if (user == null || user.AppUserId == userId)
                return true;

            return false;

        }

        public async Task<bool> BeUniqueEmail(int userId, string email)
        {
            var user = await _identityService.GetUserByEmailAsync(email);
            if (user == null || user.AppUserId == userId)
                return true;

            return false;
        }

        public async Task<bool> RolesExists(List<string> userRoles, CancellationToken token)
        {

            foreach (var role in userRoles)
            {
                var roleLookup = await _identityService.GetRoleByNameAsync(role);
                if (roleLookup == null)
                    return false;
            }
            return true;
        }
    }
}
