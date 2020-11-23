using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using _10_ProjectTye.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace _10_ProjectTye.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly Random _rng = new Random();
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDistributedCache _distributedCache;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<WeatherForecast> Get()
        {
            var cachedWeather = await _distributedCache.GetStringAsync("weather");
            WeatherForecast weather = cachedWeather is not null ? JsonSerializer.Deserialize<WeatherForecast>(cachedWeather) : null;

            if (weather is null)
            {
                weather = new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(1),
                    Temperature = _rng.Next(-20, 55),
                    Summary = Summaries[_rng.Next(Summaries.Length)]
                };

                cachedWeather = JsonSerializer.Serialize(weather);

                await _distributedCache.SetStringAsync("weather", cachedWeather, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
                });
            }

            return weather;
        }
    }
}
