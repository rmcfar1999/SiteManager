using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SiteManager.V4.Infrastructure.Identity;

namespace SiteManager.V4.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<AppUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<ActionResult> OnGetAsync(string returnUrl = null)
        {
            //RMC: Leaving these in place for potential use in a federated environment requiring non-angular/3rd party authentication.
            //for now...redirect/disable
            return Redirect("/"); 

            //if (!string.IsNullOrEmpty(ErrorMessage))
            //{
            //    ModelState.AddModelError(string.Empty, ErrorMessage);
            //}

            //returnUrl = returnUrl ?? Url.Content("~/");

            //// Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //RMC: Leaving these in place for potential use in a federated environment requiring non-angular/3rd party authentication.
            //for now...redirect/disable
            return Redirect("/");

            //returnUrl = returnUrl ?? Url.Content("~/");

            //if (ModelState.IsValid)
            //{
            //    // This doesn't count login failures towards account lockout
            //    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            //    if (result.Succeeded)
            //    {
            //        _logger.LogInformation("User logged in.");
            //        return LocalRedirect(returnUrl);
            //    }
            //    if (result.RequiresTwoFactor)
            //    {
            //        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            //    }
            //    if (result.IsLockedOut)
            //    {
            //        _logger.LogWarning("User account locked out.");
            //        return RedirectToPage("./Lockout");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //        return Page();
            //    }
            //}

            //// If we got this far, something failed, redisplay form
            //return Page();
        }
    }
}
