using SiteManager.V4.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace SiteManager.V4.Application.Account.Commands
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService; 

        public ForgotPasswordCommandValidator(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;

            RuleFor(v => v.Email)
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(v => v.PasswordResetUrl)
               .NotEmpty().WithMessage("Password reset URL is required.");
            ////Note these are checked within Identity directly but added here as a higher 
            ////level check/standard validation
            //RuleFor(v => v.UserName)
            //   .Must((o, ForgotPasswordCommand) => { return BeUniqueUserName(o.UserName).Result; })
            //   .WithMessage("This username is taken by another user.");

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
