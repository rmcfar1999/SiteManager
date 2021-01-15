using MediatR;
using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteManager.V4.Application.UserAdmin.Commands
{
    public partial class CreateUserCommand : IRequest<int>
    {
        public CreateUserCommand()
        {
            AppRoles = new List<string>(); 
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> AppRoles { get; set; }
        public bool SendRegistrationConfirmation { get; set; }
        public string ConfirmationUrl { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateUserCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var r = await _identityService.CreateUserAsync(request.UserName, request.Email, request.PhoneNumber, request.Password);
            if (!r.Result.Succeeded)
            {
                throw new ValidationException("User", r.Result.Errors);
            }

            if (request.SendRegistrationConfirmation)
                await _identityService.SendRegistrationConfirmation(r.UserId, request.ConfirmationUrl ); 

            var user = await _identityService.GetUserAsync(r.UserId);
            user.PhoneNumber = request.PhoneNumber;
            user.UserName = request.UserName;
            user.Email = request.Email; 
            r = await _identityService.UpdateUser(user.AppUserId, user.UserName, user.Email, user.PhoneNumber);

            if (r.Result.Succeeded)
            {
                foreach (var role in request.AppRoles)
                {
                    r = await _identityService.RemoveUserRole(user.AppUserId, role);
                    r = await _identityService.AddUserRole(user.AppUserId, role);
                }
            }

            return r.UserId;
        }
    }
}
