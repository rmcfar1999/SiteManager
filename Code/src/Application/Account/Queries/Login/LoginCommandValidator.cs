using SiteManager.V4.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace SiteManager.V4.Application.Account.Queries
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService; 

        public LoginCommandValidator(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required.");

        }

        //public async Task<bool> BeUniqueRoleName(string RoleName)
        //{
        //    var Role = await _identityService.GetRoleByNameAsync(RoleName);
        //    return Role == null;
        //}

       
    }
}
