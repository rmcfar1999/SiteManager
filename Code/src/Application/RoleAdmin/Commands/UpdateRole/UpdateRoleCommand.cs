using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Application.Common.Exceptions;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SiteManager.V4.Application.RoleAdmin.Commands
{
    public partial class UpdateRoleCommand : IRequest<int>
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, int>
    {
        //private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService; 

        public UpdateRoleCommandHandler(IIdentityService identityService)
        {
            //_context = context;
            _identityService = identityService; 
        }

        public async Task<int> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var appRoleName = await _identityService.GetRoleAsync(request.RoleId); 

            if (string.IsNullOrWhiteSpace(appRoleName.Name))
            {
                throw new NotFoundException("AppRole", request.RoleId);
            }

            var result = await _identityService.UpdateRoleAsync(request.RoleId, request.RoleName);

            return result.RoleId;
        }
    }
}
