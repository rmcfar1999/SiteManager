using System;
using System.Collections.Generic;
using SiteManager.V4.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;
using ValidationException = SiteManager.V4.Application.Common.Exceptions.ValidationException;

namespace SiteManager.V4.Application.Common.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger, IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _validators = validators;
            _logger = logger;
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var requestName = typeof(TRequest).Name;
                    var userId = _currentUserService.UserId;
                    string userName = string.Empty;

                    if (userId > 0)
                    {
                        userName = await _identityService.GetUserNameAsync(userId);
                    }

                    var ex =  new ValidationException(failures);
                    _logger.LogError(ex, "API validation error (enable debug logging for request parameters): {Name} {@UserId} {@UserName} {@Request} {failures}",
                requestName, userId, userName, request, failures);
                    Type myType = request.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    foreach (PropertyInfo prop in props)
                    {
                        object propValue = prop.GetValue(request, null);
                        _logger.LogDebug("API Params: {Property} : {@Value}", prop.Name, propValue);
                        // Do something with propValue
                    }
                    
                    throw ex; 
                }
            }
            return await next();
        }
    }
}