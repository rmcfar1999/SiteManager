using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiteManager.V4.Application.UserAdmin.Queries;
using SiteManager.V4.Application.UserAdmin.Models;
using SiteManager.V4.Application.UserAdmin.Commands;
using SiteManager.V4.Application.Common.Models;
using System.Collections.Generic; 

namespace SiteManager.V4.WebUI.Controllers
{
    [Authorize]
    public class UserAdminController : ApiController
    {
        [Authorize(Policy = "Read")]
        [HttpGet()]
        public async Task<IEnumerable<AppUserDto>> Get()
        {
            return await Mediator.Send(new GetAllUsersQuery());
        }

        [Authorize(Policy = "Read")]
        [HttpGet("{id}")]
        public async Task<IEnumerable<AppUserDto>> GetByRole(int id)
        {
            return await Mediator.Send(new GetUsersInRoleQuery() { RoleId = id });
        }

        [Authorize(Policy = "Create")]
        [HttpPost]
        public async Task<int> Create(CreateUserCommand command)
        {
            //url for the current sites email confirmation processor.
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity"},
                protocol: Request.Scheme);
            command.ConfirmationUrl = callbackUrl;
            var r = await Mediator.Send(command);
            return r;
        }

        [Authorize(Policy = "Update")]
        [HttpPut]
        public async Task<int> Update(UpdateUserCommand command)
        {
            string returnUrl = "authentication/userlogin";
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", returnUrl = returnUrl },
                protocol: Request.Scheme);
            command.PasswordResetUrl = callbackUrl;

            var r = await Mediator.Send(command);
            return r;
        }

        [Authorize(Policy = "Delete")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var r = await Mediator.Send(new DeleteUserCommand() { UserId = id });
            return NoContent();
        }


    }
}
