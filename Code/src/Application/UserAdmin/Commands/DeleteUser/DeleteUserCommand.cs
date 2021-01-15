using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiteManager.V4.Application.Common.Exceptions;
using System.Linq;

namespace SiteManager.V4.Application.UserAdmin.Commands
{
    public partial class DeleteUserCommand : IRequest
    {
        public int UserId { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IIdentityService _identityService;

        public DeleteUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var appuser = await _identityService.GetUserNameAsync(request.UserId);

            if (string.IsNullOrWhiteSpace(appuser))
            {
                throw new NotFoundException("AppUser", request.UserId);
            }

            await _identityService.DeleteUserAsync(request.UserId); 

            return Unit.Value;

        }
    }
}
