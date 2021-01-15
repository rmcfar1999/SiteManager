using MediatR;
using MediatR.Pipeline;
using SiteManager.V4.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SiteManager.V4.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService; 

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger, IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId;
            string userName = string.Empty;

            if (userId > 0)
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            //Request
            _logger.LogDebug($"Begin Request {requestName} (Enable trace level logging for request parameters)");
            Type myType = request.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(request, null);
                _logger.LogTrace("{Property} : {@Value}", prop.Name, propValue);
                // Do something with propValue
            }

            var response = await next();

            //Response
            //_logger.LogInformation($"Handled {typeof(TResponse).Name}");
            _logger.LogDebug("End Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);

            return response;
        }
    }

}
