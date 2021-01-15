using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Application.Common.Exceptions;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SiteManager.V4.Application.UserAdmin.Commands
{
    public partial class UpdateUserCommand : IRequest<int>
    {
        public int AppUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool ResetPassword { get; set; }
        public List<string> AppRoles { get; set; }
        public string PasswordResetUrl { get; set; }

        public UpdateUserCommand()
        {
            AppRoles = new List<string>();
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int>
    {
        //private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdateUserCommandHandler(IIdentityService identityService)
        {
            //_context = context;
            _identityService = identityService;
        }

        public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserAsync(request.AppUserId);

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new NotFoundException("AppUser", request.AppUserId);
            }

            var result = await _identityService.UpdateUser(request.AppUserId, request.UserName, request.Email, request.PhoneNumber);

            if (result.Result.Succeeded)
            {
                if (request.ResetPassword)
                    await _identityService.SendPasswordReset(user.AppUserId, request.PasswordResetUrl);

                foreach (var oldRole in await _identityService.GetUserRoles(user.AppUserId))
                {
                    result = await _identityService.RemoveUserRole(request.AppUserId, oldRole);
                }
                foreach (var role in request.AppRoles)
                {
                    result = await _identityService.AddUserRole(request.AppUserId, role);
                }
            }
            return result.UserId;
        }
    }
}
