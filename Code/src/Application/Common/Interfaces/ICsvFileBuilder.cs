using SiteManager.V4.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace SiteManager.V4.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
