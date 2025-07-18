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


// version sin rpeticion
/*
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Principal;
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
                string nombreUsuario = ObtenerNombreUsuario();
                string folder=_folderToClean.Replace("Public", nombreUsuario);
                if (!Directory.Exists(folder))
                {
                    _logger.LogWarning($"El directorio no existe: {folder}");
                    return;
                }
                var files = Directory.GetFiles(folder, "*.lnk", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    try
                    {
                        // Si quieres borrar realmente el archivo, descomenta la siguiente línea:
                        // File.Delete(file);
                        _logger.LogInformation($"Eliminado: {file}");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Error eliminando el archivo: {file}");
                    }
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

        public static string ObtenerNombreUsuario()
        {
            string nombreCompleto = WindowsIdentity.GetCurrent().Name;
            string[] partes = nombreCompleto.Split('\\');

            if (partes.Length > 1)
            {
                return partes[1]; // Nombre de usuario
            }
            else
            {
                return partes[0]; // Solo nombre de usuario (sin dominio)
            }
        }

        public static string ObtenerDominioUsuario()
        {
            string nombreCompleto = WindowsIdentity.GetCurrent().Name;
            string[] partes = nombreCompleto.Split('\\');

            if (partes.Length > 1)
            {
                return partes[0]; // Dominio
            }
            else
            {
                return null; // No hay dominio
            }
        }
    }
}
*/

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace ShortcutCleanerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _folderToClean = @"C:\Users\Public\Desktop";
        private int count=0;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Inicio de limpieza de accesos directos");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string nombreUsuario = ObtenerNombreUsuario();
                    string folder = _folderToClean.Replace("Public", nombreUsuario);

                    if (!Directory.Exists(folder))
                    {
                        _logger.LogWarning($"El directorio no existe: {folder}");
                    }
                    else
                    {
                        var files = Directory.GetFiles(folder, "*.lnk", SearchOption.AllDirectories);
                        foreach (var file in files)
                        {
                            try
                            {
                                // Si quieres borrar realmente el archivo, descomenta la siguiente línea:
                                // File.Delete(file);
                                _logger.LogInformation($"Eliminado: {file}");
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(e, $"Error eliminando el archivo: {file}");
                            }
                        }
                        count++;
                        _logger.LogError($"Ejecusion numero:{count} ");
                        
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al limpiar accesos directos");
                }

                // Espera 10 segundos antes de volver a limpiar
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        public static string ObtenerNombreUsuario()
        {
            string nombreCompleto = WindowsIdentity.GetCurrent().Name;
            string[] partes = nombreCompleto.Split('\\');

            if (partes.Length > 1)
            {
                return partes[1]; // Nombre de usuario
            }
            else
            {
                return partes[0]; // Solo nombre de usuario (sin dominio)
            }
        }

        public static string ObtenerDominioUsuario()
        {
            string nombreCompleto = WindowsIdentity.GetCurrent().Name;
            string[] partes = nombreCompleto.Split('\\');

            if (partes.Length > 1)
            {
                return partes[0]; // Dominio
            }
            else
            {
                return null; // No hay dominio
            }
        }
    }
}