using SiteManager.V4.Application.TodoLists.Commands.CreateTodoList;
using SiteManager.V4.Application.TodoLists.Commands.DeleteTodoList;
using SiteManager.V4.Application.TodoLists.Commands.UpdateTodoList;
using SiteManager.V4.Application.TodoLists.Queries.ExportTodos;
using SiteManager.V4.Application.TodoLists.Queries.GetTodos;
using SiteManager.V4.Application.Common.Interfaces; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using SiteManager.V4.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;


namespace SiteManager.V4.WebUI.Controllers
{
    //[Authorize(Policy = "InRole")]
    public class TodoListsController : ApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICurrentUserService _currentUserService; 

        public TodoListsController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _currentUserService = currentUserService;
        }

        [Authorize(Policy = "Read")]
        [HttpGet]
        public async Task<ActionResult<TodosVm>> Get()
        {
            return await Mediator.Send(new GetTodosQuery());
        }

        [Authorize(Policy = "Read")]
        [HttpGet("{id}")]
        public async Task<FileResult> Get(int id)
        {
            var vm = await Mediator.Send(new ExportTodosQuery { ListId = id });

            return File(vm.Content, vm.ContentType, vm.FileName);
        }

        [Authorize(Policy = "Create")]
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateTodoListCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Policy = "Update")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTodoListCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [Authorize(Policy = "Delete")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTodoListCommand { Id = id });

            return NoContent();
        }
    }
}
