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

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager; 
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public IdentityService(IApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IEmailService emailService)
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

        public async Task<AppUserDto> GetUserAsync(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null; 

            return new AppUserDto() { 
                AppUserId = user.Id, 
                Email = user.Email, 
                PhoneNumber = user.PhoneNumber, 
                UserName = user.UserName
            };
        }

        public async Task<AppUserDto> GetUserAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());
            if (user == null)
                return null;

            return new AppUserDto()
            {
                AppUserId = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed
            };
        }
        
        public async Task<AppUserDto> GetUserByEmailAsync(string userEmail)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == userEmail.ToUpper());
            if (user == null)
                return null;

            return new AppUserDto()
            {
                AppUserId = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                EmailConfirmed = user.EmailConfirmed
            };
        }

        public async Task<IList<string>> GetUserRoles(int appUserId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == appUserId); 
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
        public async Task<IList<AppUserDto>> GetUsersInRoleAsync(int roleId)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
            if (role == null)
                throw new NotFoundException("AppRole", roleId); 

            var users = await _userManager.GetUsersInRoleAsync(role.Name);

            var r = users.Select(x => new AppUserDto() { 
                AppUserId = x.Id, 
                Email = x.Email, 
                EmailConfirmed = x.EmailConfirmed, 
                PhoneNumber = x.PhoneNumber, 
                UserName = x.UserName,
                AppRoles = _userManager.GetRolesAsync(x).Result
            });

            return r.ToList(); 
        }
        public async Task<(Result Result, int UserId)> UpdateUser(int userId, string userName, string email, string phoneNumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return (Result.Failure(new List<string>() { "User not found"}), userId);


            if (user.Email.ToLower() != email)
                user.EmailConfirmed = false; 

            user.Email = email;
            user.PhoneNumber = phoneNumber;
            user.UserName = userName; 

            var result = await _userManager.UpdateAsync(user);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<IEnumerable<AppUserDto>> GetAllUsers()
        {

            var users = _userManager.Users.ToList();
            
            var r = users.Select(x => new AppUserDto() { 
                AppUserId = x.Id, 
                UserName = x.UserName, 
                Email = x.Email, 
                EmailConfirmed = x.EmailConfirmed, 
                PhoneNumber = x.PhoneNumber,
                AppRoles = _userManager.GetRolesAsync(x).Result.ToList()
            }).ToList();

            return await Task.FromResult(r); 
        }

        public async Task<(Result Result, int UserId)> CreateUserAsync(string userName, string Email, string phone, string password)
        {
            var user = new AppUser
            {
                UserName = userName,
                Email = Email,
                PhoneNumber = phone
            };

            var result = await _userManager.CreateAsync(user, password);
            
            if (result.Succeeded)
            {
                //isolated this confirmation email to specific sendregistrationconfirmation method
                //var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //confirmationToken = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(confirmationToken));

                //var emailTemplate = SiteManager.V4.Domain.Properties.Resources.ConfirmEmail;
                //emailTemplate = String.Format(emailTemplate, user.Id, confirmationToken);
                //_emailService.Send(user.Email, "Account EMail Verification", emailTemplate); 
            }

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<Result> DeleteUserAsync(int userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(AppUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<Result> SendPasswordReset(int UserId, string resetPasswordUrl)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordUrl))
            {
                throw new ValidationException("ResetPassword", new string[] { "Password reset URL missing" });
            }
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            //var result = await _userManager.ResetAccessFailedCountAsync(user);
            if (user == null)
                throw new NotFoundException("AppUser", UserId);

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            resetToken = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(resetToken));

            var param = new Dictionary<string, string>() { { "code", resetToken } };
            param.Add("userid", user.Id.ToString());

            var callBackUrl = new Uri(QueryHelpers.AddQueryString(resetPasswordUrl, param));

            var emailTemplate = SiteManager.V4.Domain.Properties.Resources.ResetPassword;
            emailTemplate = String.Format(emailTemplate, callBackUrl); 
            _emailService.Send(user.Email, "Your Password Reset Request", emailTemplate);

            return Result.Success();

        }

        public async Task SendRegistrationConfirmation(int userId, string registrationUrl)
        {
            if (string.IsNullOrWhiteSpace(registrationUrl))
            {
                throw new ValidationException("Registration", new string[] { "Registration URL missing" });
            }
            var user = _userManager.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(code));

                var param = new Dictionary<string, string>() { { "code", code } };
                param.Add("userid", user.Id.ToString() );

                var callBackUrl = new Uri(QueryHelpers.AddQueryString(registrationUrl, param));

                var emailTemplate = SiteManager.V4.Domain.Properties.Resources.ConfirmEmail;
                emailTemplate = String.Format(emailTemplate, callBackUrl);

                _emailService.Send(user.Email, "Registration Confirmation", emailTemplate);
                
            }
            else
                throw new ValidationException("User", new string[] { "User Not Found" }); 

        }

        public async Task<(Result Result, int UserId)> AddUserRole(int UserId, string role)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            //var result = await _userManager.ResetAccessFailedCountAsync(user);
            if (user == null)
                throw new NotFoundException("AppUser", UserId);

            var result = await _userManager.AddToRoleAsync(user, role);

            return (result.ToApplicationResult(), user.Id);

        }

        public async Task<(Result Result, int UserId)> RemoveUserRole(int UserId, string role)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            //var result = await _userManager.ResetAccessFailedCountAsync(user);
            if (user == null)
                throw new NotFoundException("AppUser", UserId);

            var result = await _userManager.RemoveFromRoleAsync(user, role); 

            return (result.ToApplicationResult(), user.Id);

        }
        public async Task<Result> DeleteRoleAsync(int RoleId)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == RoleId);
            var result = await _roleManager.DeleteAsync(role); 

            return result.ToApplicationResult();
        }

        public async Task<(Result Result, int RoleId)> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new AppRole() { Name = roleName});
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.NormalizedName == roleName.ToUpper());
            return (result.ToApplicationResult(), role.Id);
        }

        public async Task<(Result Result, int RoleId)> UpdateRoleAsync(int RoleId, string roleName)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == RoleId);
            role.Name = roleName; 
            
            var result = await _roleManager.UpdateAsync(role);

            return (result.ToApplicationResult(), RoleId);
        }

        public async Task<AppRoleDto> GetRoleAsync(int RoleId)
        {
            var result = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == RoleId);
            if (result != null)
                return new AppRoleDto() { AppRoleId = result.Id, Name = result.Name };
            else
                return null;
        }

        public async Task<AppRoleDto> GetRoleByNameAsync(string roleName)
        {
            var result = await _roleManager.Roles.FirstOrDefaultAsync(x => x.NormalizedName == roleName.ToUpper());
            if (result != null)
                return new AppRoleDto() { AppRoleId = result.Id, Name = result.Name };
            else
                return null; 
        }

        public async Task<string> GetRoleNameAsync(int RoleId)
        {
            var result = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == RoleId);
            if (result != null)
                return result.Name;
            else
                return null;
        }

        public async Task<IEnumerable<AppRoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var r = roles.Select(x => new AppRoleDto() { AppRoleId = x.Id, Name = x.Name });
            return r;
        }
    }
}
