using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SiteManager.V4.Infrastructure.Identity;

namespace SiteManager.V4.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordConfirmationModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
            
        }

        
    }
}
