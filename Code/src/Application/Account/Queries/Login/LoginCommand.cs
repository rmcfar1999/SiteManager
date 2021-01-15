using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Application.Common.Exceptions;
using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SiteManager.V4.Application.Account.Queries
{
    public partial class LoginCommand : IRequest<int>
    {
        public LoginCommand()
        {
        }
        public string ReturnUrl { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<int> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            //if (result.Succeeded)
            //{
            //    _logger.LogInformation("User logged in.");
            //    return LocalRedirect(returnUrl);
            //}
            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            //}
            //if (result.IsLockedOut)
            //{
            //    _logger.LogWarning("User account locked out.");
            //    return RedirectToPage("./Lockout");
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //    return Page();
            //}
            return 1;
        }
    }
}
