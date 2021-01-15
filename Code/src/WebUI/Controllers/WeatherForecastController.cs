using SiteManager.V4.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiteManager.V4.Application.Common.Behaviours;
using System.Threading;

namespace SiteManager.V4.WebUI.Controllers
{
    [Authorize(Policy = "IsPublic")]
    public class WeatherForecastController : ApiController
    {
        ILogger _logger; 
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
           
            _logger.LogError("Test Error");

            return await Mediator.Send(new GetWeatherForecastsQuery());
        }
    }
}
