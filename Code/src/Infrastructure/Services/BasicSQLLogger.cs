using System;
using System.Collections.Generic;
using System.Text;
using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;
using System.Text.Json;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SiteManager.V4.Infrastructure.Services
{
    //https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
    //Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5, and None = 6.
    public class BasicSQLLogProvider : ILoggerProvider
    {
        private IConfiguration _configuration;
        private ICurrentUserService _currentUserService;
        private IOptions<OperationalStoreOptions> _operationalStoreOptions;
        private IDateTime _dateTime; 

        public BasicSQLLogProvider(
            IConfiguration configuration, 
            ICurrentUserService currentUserService,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IDateTime dateTime)
        {
            _configuration = configuration;
            _currentUserService = currentUserService;
            _operationalStoreOptions = operationalStoreOptions;
            _dateTime = dateTime;

        }
        public ILogger CreateLogger(string categoryName)
        {
            return new BasicSQLLogger(_configuration, _currentUserService, _operationalStoreOptions, _dateTime, categoryName, string.Empty);
        }

        public void Dispose()
        {
        }
    }
    public class BasicSQLLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly string _logPrefix;
        private IConfiguration _configuration;
        private ICurrentUserService _currentUserService;
        private IOptions<OperationalStoreOptions> _operationalStoreOptions;
        private IDateTime _dateTime;

        public BasicSQLLogger(
            IConfiguration configuration, 
            ICurrentUserService currentUserService,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IDateTime dateTime, 
            string categoryName, 
            string logPrefix)
        {
            _categoryName = categoryName;
            _logPrefix = logPrefix;
            _configuration = configuration;
            _currentUserService = currentUserService;
            _operationalStoreOptions = operationalStoreOptions;
            _dateTime = dateTime;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string message = _logPrefix;
            string jsonError = "{\"Log Serialization Error\":\"Unable to serialize error data to json\"}";

            try
            {
                jsonError = JsonConvert.SerializeObject(new
                {
                    logLevel,
                    eventId,
                    parameters = (state as IEnumerable<KeyValuePair<string, object>>)?.ToDictionary(i => i.Key, i => i.Value),
                    message = formatter(state, exception),
                    exception = exception?.GetType().Name
                });
            } catch (Exception ex) //todo: State can be highly complex need to just skip this for now while we sort through log consumers and what they send
            {
                System.Diagnostics.Debug.WriteLine($"An error occured serializing error data to JSON for logging: {jsonError} {ex.Message} {ex.StackTrace}");
            }

            if (formatter != null)
            {
                message += formatter(state, exception);
            }

            if (!eventId.ToString().Contains("EntityFramework"))
            {
                AppendDatabase(logLevel, message, eventId, exception, _currentUserService.UserId.ToString(), jsonError);
            }
            //System.Diagnostics.Debug.WriteLine($"{logLevel.ToString()} - {eventId.Id} - {_categoryName} - {message}");
            //System.Diagnostics.Debug.WriteLine(jsonError);
        }

        private async void AppendDatabase(LogLevel logLevel, string logMessage, EventId eventId, Exception exception, string userName, string jsonData)
        {
            var dbLog = new AppLog()
            {
                LogDateTime = DateTime.Now, 
                EventId = eventId.Name, 
                LogLevel = logLevel.ToString(), 
                Category = _categoryName,
                LogMessage = logMessage, 
                //StateInfo = state, 
                LogException = exception?.StackTrace, 
                UserName = userName, 
                JsonData = jsonData
            };

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            builder.UseNpgsql(connectionString);
            
            //builder.UseSqlServer(connectionString);

            using (var ctx = new ApplicationDbContext(builder.Options,
                    _operationalStoreOptions,
                    _currentUserService,
                    _dateTime))
            {
                ctx.AppLog.Add(dbLog);
                var result = await ctx.SaveChangesAsync(new System.Threading.CancellationToken());
            }
        }
        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
