using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiteManager.V4.Application.Common.Exceptions;
using System.Linq;

namespace SiteManager.V4.Application.RoleAdmin.Commands
{
    public partial class DeleteRoleCommand : IRequest
    {
        public int RoleId { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
    {
        private readonly IIdentityService _identityService;

        public DeleteRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var appRole = await _identityService.GetRoleNameAsync(request.RoleId);

            if (string.IsNullOrWhiteSpace(appRole))
            {
                throw new NotFoundException("AppRole", request.RoleId);
            }

            await _identityService.DeleteRoleAsync(request.RoleId); 

            return Unit.Value;

        }
    }
}
