using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteManager.V4.Application.Account.Queries;
using SiteManager.V4.Application.Account.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SiteManager.V4.Infrastructure.Identity;
using SiteManager.V4.Application.Common.Models;

namespace SiteManager.V4.WebUI.Controllers
{
    public class AccountController : ApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
           // _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginCommand login)
        {
            string returnUrl = login.ReturnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User logged in.");
                    return new OkObjectResult(result); // RedirectToPage(login.ReturnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = login.ReturnUrl, RememberMe = login.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    //_logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    throw new Exception("Login Failed");
                }
            }
            return RedirectToAction("/");
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<int> RegisterAsync(RegisterCommand register)
        {

            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string returnUrl = register.ReturnUrl ?? Url.Content("~/");
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", returnUrl = returnUrl },
                protocol: Request.Scheme);
            register.ConfirmationUrl = callbackUrl; 
            var r = await Mediator.Send(register);
            return r;
        }

        [HttpPost]
        [Route("forgotpassword")]
        [AllowAnonymous]
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordCommand register)
        {
            bool r = true; //always send true to stop email fishing.
            try
            {
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //https://localhost:44312/Identity/Account/ResetPassword?userid={0}&code={1}
                string returnUrl = "authentication/userlogin";
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", returnUrl = returnUrl },
                    protocol: Request.Scheme);
                register.PasswordResetUrl = callbackUrl;
                var result = await Mediator.Send(register);
                return r;
            } catch(Exception ex)
            {
                //do not notify of issues
                return r; 
            }
        }
    }
}
