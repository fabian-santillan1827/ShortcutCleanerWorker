using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging.EventLog;

namespace ShortcutCleanerWorker
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    Host.CreateDefaultBuilder(args)
        //        .UseWindowsService()
        //        .ConfigureServices((hostContext, services) =>
        //        {
        //            services.AddHostedService<Worker>();
        //        })
        //        .Build()
        //        .Run();
        //}

        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddEventLog(settings =>
                    {
                        settings.SourceName = "ShortcutCleanerWorker";
                    });
                    logging.AddFilter<EventLogLoggerProvider>("", LogLevel.Information);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .Build()
                .Run();
        }
    }
}