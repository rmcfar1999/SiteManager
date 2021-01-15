using SiteManager.V4.Application.Common.Mappings;
using SiteManager.V4.Domain.Entities;
using System.Collections.Generic;

namespace SiteManager.V4.Application.TodoLists.Queries.GetTodos
{
    public class TodoListDto : IMapFrom<TodoList>
{
    public TodoListDto()
    {
        Items = new List<TodoItemDto>();
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public IList<TodoItemDto> Items { get; set; }
}
}
