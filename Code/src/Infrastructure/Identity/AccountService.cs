using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Application.Common.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using System;
using SiteManager.V4.Application.Common.Exceptions;
using Org.BouncyCastle.Math.EC.Rfc7748;
using MimeKit.Encodings;

namespace SiteManager.V4.Infrastructure.Identity
{
    public class AccountService_DEP
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AccountService_DEP(IApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _emailService = emailService;
        }

        public async Task<string> GetUserNameAsync(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user?.UserName;
        }
    }
}
