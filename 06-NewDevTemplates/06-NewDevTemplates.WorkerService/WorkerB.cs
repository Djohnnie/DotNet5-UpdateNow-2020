using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace _06_NewDevTemplates.WorkerService
{
    public class WorkerB : BackgroundService
    {
        private readonly ILogger<WorkerA> _logger;

        public WorkerB(ILogger<WorkerA> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("WorkerB running at: {time}", DateTimeOffset.Now);
                await Task.Delay(3333, stoppingToken);
            }
        }
    }
}