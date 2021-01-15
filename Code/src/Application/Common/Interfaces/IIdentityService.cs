using SiteManager.V4.Application.Common.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SiteManager.V4.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(Result Result, int UserId)> CreateUserAsync(string userName, string Email, string phone, string password);
        Task<(Result Result, int UserId)> UpdateUser(int userId, string userName, string email, string phoneNumber);
        //Task<Result> DeleteUserAsync(AppUser user);
        Task<Result> DeleteUserAsync(int userId);
        Task<string> GetUserNameAsync(int userId);
        Task<AppUserDto> GetUserAsync(int userId);
        Task<AppUserDto> GetUserAsync(string userName);
        Task<AppUserDto> GetUserByEmailAsync(string userEmail);
        Task<IEnumerable<AppUserDto>> GetAllUsers();
        Task<Result> DeleteRoleAsync(int roleId);
        Task<(Result Result, int UserId)> AddUserRole(int UserId, string role);
        Task<(Result Result, int UserId)> RemoveUserRole(int UserId, string role);
        Task<(Result Result, int RoleId)> CreateRoleAsync(string roleName);
        Task<(Result Result, int RoleId)> UpdateRoleAsync(int roleId, string roleName);
        Task<AppRoleDto> GetRoleAsync(int roleId);
        Task<IEnumerable<AppRoleDto>> GetAllRolesAsync();
        Task<string> GetRoleNameAsync(int roleId);
        Task<AppRoleDto> GetRoleByNameAsync(string roleName);
        Task<IList<AppUserDto>> GetUsersInRoleAsync(int roleId);
        Task SendRegistrationConfirmation(int userId, string registrationUrl);
        Task<Result> SendPasswordReset(int UserId, string resetPasswordUrl);
        Task<IList<string>> GetUserRoles(int appUserId);
    }
    //public interface IIdentityService
    //{
    //    Task<string> GetUserNameAsync(int userId);

    //    Task<(Result Result, int UserId)> CreateUserAsync(string userName, string password);

    //    Task<Result> DeleteUserAsync(int userId);
    //}
}
