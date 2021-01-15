using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiteManager.V4.Application.RoleAdmin.Queries;
using SiteManager.V4.Application.RoleAdmin.Models;
using SiteManager.V4.Application.RoleAdmin.Commands;
using SiteManager.V4.Application.Common.Models;
using System.Collections.Generic; 

namespace SiteManager.V4.WebUI.Controllers
{
    [Authorize]
    public class RoleAdminController : ApiController
    {
        [Authorize(Policy = "Read")]
        [HttpGet()]
        public async Task<IEnumerable<AppRoleDto>> Get()
        {
            return await Mediator.Send(new GetAllRolesQuery());
        }

        [Authorize(Policy = "Create")]
        [HttpPost]
        public async Task<int> Create(CreateRoleCommand command)
        {
            var r = await Mediator.Send(command);
            return r;
        }

        [Authorize(Policy = "Update")]
        [HttpPut]
        public async Task<int> Update(UpdateRoleCommand command)
        {
            var r = await Mediator.Send(command);
            return r;
        }

        [Authorize(Policy = "Delete")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var r = await Mediator.Send(new DeleteRoleCommand() { RoleId = id });
            return NoContent();
        }


    }
}
