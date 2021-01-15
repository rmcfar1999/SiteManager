using SiteManager.V4.Application.Common.Mappings;
using SiteManager.V4.Domain.Entities;

namespace SiteManager.V4.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
