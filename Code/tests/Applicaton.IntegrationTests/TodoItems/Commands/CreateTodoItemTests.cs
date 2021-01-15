using SiteManager.V4.Application.Common.Exceptions;
using SiteManager.V4.Application.TodoItems.Commands.CreateTodoItem;
using SiteManager.V4.Application.TodoLists.Commands.CreateTodoList;
using SiteManager.V4.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace SiteManager.V4.Application.IntegrationTests.TodoItems.Commands
{
    using static Testing;

    public class CreateTodoItemTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateTodoItemCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateTodoItem()
        {
            var userId = await RunAsDefaultUserAsync();

            var listId = await SendAsync(new CreateTodoListCommand
            {
                Title = "New List"
            });

            var command = new CreateTodoItemCommand
            {
                ListId = listId,
                Title = "Tasks"
            };

            var itemId = await SendAsync(command);

            var item = await FindAsync<TodoItem>(itemId);

            item.Should().NotBeNull();
            item.ListId.Should().Be(command.ListId);
            item.Title.Should().Be(command.Title);
            item.CreatedBy.Should().Be(userId);
            item.Created.Should().BeCloseTo(DateTime.Now, 10000);
            item.LastModifiedBy.Should().Be(0); 
            item.LastModified.Should().BeNull();
        }
    }
}
