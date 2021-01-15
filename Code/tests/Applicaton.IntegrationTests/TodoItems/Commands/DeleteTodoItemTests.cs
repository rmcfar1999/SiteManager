using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.TodoItems.Commands.CreateTodoItem;
using SiteManager.V4.Application.TodoItems.Commands.DeleteTodoItem;
using SiteManager.V4.Application.TodoLists.Commands.CreateTodoList;
using SiteManager.V4.Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SiteManager.V4.Application.IntegrationTests.TodoItems.Commands
{
    using static Testing;

    public class DeleteTodoItemTests : TestBase
    {
        [Test]
        public void ShouldRequireValidTodoItemId()
        {
            var command = new DeleteTodoItemCommand { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteTodoItem()
        {
            var listId = await SendAsync(new CreateTodoListCommand
            {
                Title = "New List"
            });

            var itemId = await SendAsync(new CreateTodoItemCommand
            {
                ListId = listId,
                Title = "New Item"
            });

            await SendAsync(new DeleteTodoItemCommand
            {
                Id = itemId
            });

            var list = await FindAsync<TodoItem>(listId);

            list.Should().BeNull();
        }
    }
}
