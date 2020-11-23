using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _06_NewDevTemplates.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<WorkerA>();
                    services.AddHostedService<WorkerB>();
                });
    }
}