using SiteManager.V4.Application.Common.Behaviours;
using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Application.TodoItems.Commands.CreateTodoItem;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace SiteManager.V4.Application.UnitTests.Common.Behaviours
{
    public class RequestLoggerTests
    {
        private readonly Mock<ILogger<CreateTodoItemCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IIdentityService> _identityService;


        public RequestLoggerTests()
        {
            _logger = new Mock<ILogger<CreateTodoItemCommand>>();

            _currentUserService = new Mock<ICurrentUserService>();

            _identityService = new Mock<IIdentityService>();
        }

        //ToDo: replace these with better coverage in application?  These are not really
        //pertinent after new logging behaviour changes. 
        //[Test]
        //public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        //{
        //    _currentUserService.Setup(x => x.UserId).Returns(1);

        //    var requestLogger = new LoggingBehaviour<CreateTodoItemCommand,>(_logger.Object, _currentUserService.Object, _identityService.Object);

        //    await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

        //    _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<int>()), Times.Once);
        //}

        //[Test]
        //public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        //{
        //    var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

        //    await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

        //    _identityService.Verify(i => i.GetUserNameAsync(0), Times.Never);
        //}
    }
}
