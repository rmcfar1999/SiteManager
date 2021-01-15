using SiteManager.V4.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace SiteManager.V4.Application.RoleAdmin.Commands
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdateRoleCommandValidator(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;

            RuleFor(v => v.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(256).WithMessage("Role name must be less than 256 characters.")
                .Must((o, UpdateRoleCommand) => { return BeUniqueRoleName(o.RoleId, o.RoleName).Result; })
                .WithMessage("This role already exists.");
        }

        public async Task<bool> BeUniqueRoleName(int RoleId, string RoleName)
        {
            var Role = await _identityService.GetRoleByNameAsync(RoleName);
            if (Role == null || Role.AppRoleId == RoleId)
                return true;

            return false;
            
        }

    }
}
