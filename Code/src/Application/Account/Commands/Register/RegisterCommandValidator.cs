using SiteManager.V4.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace SiteManager.V4.Application.Account.Commands
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService; 

        public RegisterCommandValidator(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;

            RuleFor(v => v.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(256).WithMessage("Question name must be less than 256 characters.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(v => v.Email)
                .EmailAddress().WithMessage("Email is not valid.")
                .MaximumLength(256).WithMessage("Question comments must be less than 150 characters.");

            RuleFor(v => v.ConfirmationUrl)
               .NotEmpty().WithMessage("User confirmation URL is required.");

            //Note these are checked within Identity directly but added here as a higher 
            //level check/standard validation
            RuleFor(v => v.UserName)
               .Must((o, RegisterCommand) => { return BeUniqueUserName(o.UserName).Result; })
               .WithMessage("This username is taken by another user.");

            RuleFor(v => v.Password)
               .Must((o, RegisterCommand) => { return PasswordMatch(o.Password, o.ConfirmPassword); })
               .WithMessage("Password and confirm password do not match.");

            RuleFor(v => v.Email)
               .Must((o, RegisterCommand) => { return BeUniqueEmail(o.Email).Result; })
               .WithMessage("This email is taken by another user.");

            //RuleFor(v => v.AppRoles)
            //    .NotEmpty().WithMessage("Roles are required for users.  Use 'Public' if unsure.")
            //    .WithMessage("A role specified was not found in the system.");
        }

        public bool PasswordMatch(string password, string confirmPassword)
        {
            return password == confirmPassword;
        }

        public async Task<bool> BeUniqueUserName(string userName)
        {
            var user = await _identityService.GetUserAsync(userName);
            return user == null;
        }

        public async Task<bool> BeUniqueEmail(string email)
        {
            var user = await _identityService.GetUserByEmailAsync(email);
            return user == null;
        }
    }
}
