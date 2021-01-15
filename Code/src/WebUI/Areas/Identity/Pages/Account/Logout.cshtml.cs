using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SiteManager.V4.Infrastructure.Identity;

namespace SiteManager.V4.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            //RMC: Leaving these in place for potential use in a federated environment requiring non-angular/3rd party authentication.
            //for now...redirect/disable
            return Redirect("/");
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            //RMC: Leaving these in place for potential use in a federated environment requiring non-angular/3rd party authentication.
            //for now...redirect/disable
            return Redirect("/");

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
