using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace _06_NewDevTemplates.GrpcService.Services
{
    public class SaunaService : Sauna.SaunaBase
    {
        private readonly ILogger<SaunaService> _logger;
        private readonly Random _randomGenerator = new Random();

        public SaunaService(ILogger<SaunaService> logger)
        {
            _logger = logger;
        }

        public override Task<SaunaResponse> FetchCurrentState(SaunaRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SaunaResponse
            {
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                IsDrySauna = _randomGenerator.Next(0, 2) == 1,
                IsInfraRed = _randomGenerator.Next(0, 2) == 1,
                Temperature = GetTemperature(request.TemperatureUnit),
                Description = ""
            });
        }

        public override async Task FetchStateStream(SaunaRequest request, IServerStreamWriter<SaunaResponse> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);

                var message = new SaunaResponse
                {
                    TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    IsDrySauna = _randomGenerator.Next(0, 2) == 1,
                    IsInfraRed = _randomGenerator.Next(0, 2) == 1,
                    Temperature = GetTemperature(request.TemperatureUnit),
                    Description = ""
                };

                if (!context.CancellationToken.IsCancellationRequested)
                {
                    await responseStream.WriteAsync(message);
                }
            }
        }

        private int GetTemperature(string unit)
        {
            int celsius = _randomGenerator.Next(19, 110) + 1;
            return unit == "F" ? celsius * 9 / 5 + 32 : celsius;
        }
    }
}