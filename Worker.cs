//namespace ShortcutCleanerWorker
//{
//    public class Worker : BackgroundService
//    {
//        private readonly ILogger<Worker> _logger;

//        public Worker(ILogger<Worker> logger)
//        {
//            _logger = logger;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                if (_logger.IsEnabled(LogLevel.Information))
//                {
//                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
//                }
//                await Task.Delay(1000, stoppingToken);
//            }
//        }
//    }
//}

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ShortcutCleanerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _folderToClean = @"C:\Users\Public\Desktop";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Inicio de limpieza de accesos directos");

            try
            {
                var files = Directory.GetFiles(_folderToClean, "*.lnk", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    //File.Delete(file);
                    _logger.LogInformation($"Eliminado: {file}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al limpiar accesos directos");
            }

            // Mantén el servicio en ejecución
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                // Si quieres limpiar cada hora, repite la limpieza aquí
                // var files = Directory.GetFiles(_folderToClean, "*.lnk", SearchOption.AllDirectories);
                // ...
            }
        }
    }
}
