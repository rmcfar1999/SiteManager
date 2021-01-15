using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Application.Common.Exceptions;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SiteManager.V4.Application.RoleAdmin.Commands
{
    public partial class CreateRoleCommand : IRequest<int>
    {
        public CreateRoleCommand()
        {
        }
        public string RoleName { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var r = await _identityService.CreateRoleAsync(request.RoleName);
            if (!r.Result.Succeeded)
            {
                throw new ValidationException("Role", r.Result.Errors);
            }
            var newRole = await _identityService.GetRoleByNameAsync(request.RoleName); 
            return newRole.AppRoleId;         
        }
    }
}
