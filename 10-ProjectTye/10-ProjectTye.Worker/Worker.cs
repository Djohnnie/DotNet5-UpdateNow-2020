using System.Threading;
using System.Threading.Tasks;
using _10_ProjectTye.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace _10_ProjectTye.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(
            ILogger<Worker> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var backendUri = _configuration.GetServiceUri("projecttye-backend");
                var restClient = new RestClient(backendUri);
                var restRequest = new RestRequest("/weatherforecast", Method.GET);
                var restResponse = await restClient.ExecuteAsync<WeatherForecast>(restRequest);

                _logger.LogInformation($"WEATHER FORECAST: {restResponse.Data}");

                await Task.Delay(2500, stoppingToken);
            }
        }
    }
}