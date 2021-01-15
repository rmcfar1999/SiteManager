using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SiteManager.V4.Application.Common.Exceptions;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SiteManager.V4.Application.Account.Commands
{
    public partial class ForgotPasswordCommand : IRequest<int>
    {
        public string Email { get; set; }
        public string PasswordResetUrl { get; set; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, int>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ForgotPasswordCommand> _logger; 

        public ForgotPasswordCommandHandler(IIdentityService identityService, ILogger<ForgotPasswordCommand> logger)
        {
            _identityService = identityService;
            _logger = logger; 
        }

        public async Task<int> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("************!!!!!!!!!!!!!!!!!!!!!!!!!Test Command Logging"); 
            var user = await _identityService.GetUserByEmailAsync(request.Email);
            if (user != null)
            {
                var r = await _identityService.SendPasswordReset(user.AppUserId, request.PasswordResetUrl); 
            }
            return user.AppUserId;
            
        }
    }
}
