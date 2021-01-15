using SiteManager.V4.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace SiteManager.V4.Application.RoleAdmin.Commands
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService; 

        public CreateRoleCommandValidator(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService; 

            RuleFor(v => v.RoleName)
                .NotEmpty().WithMessage("Rolename is required.")
                .MaximumLength(256).WithMessage("Question name must be less than 256 characters.");

            //Note these are checked within Identity directly but added here as a higher 
            //level check/standard validation
            RuleFor(v => v.RoleName)
               .Must((o, UpdateRoleCommand) => { return BeUniqueRoleName(o.RoleName).Result; })
               .WithMessage("This Rolename is taken by another Role.");
        }

        public async Task<bool> BeUniqueRoleName(string RoleName)
        {
            var Role = await _identityService.GetRoleByNameAsync(RoleName);
            return Role == null;
        }

       
    }
}
