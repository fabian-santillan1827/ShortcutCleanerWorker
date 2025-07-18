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
        //private readonly string _folderAdd = @"C:\sys\newshortcut";
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
                    string rutaEscritorioUsuario = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        "Desktop");

                    string folderAdd = Path.Combine(@"C:\sys", "newshortcut");

                    if (!Directory.Exists(rutaEscritorioUsuario))
                    {
                        _logger.LogWarning($"El directorio no existe: {rutaEscritorioUsuario}");
                        count++;
                        _logger.LogInformation($"Ejecusion numero:{count} ");
                    }
                    else
                    {
                        RemoveShortCut(rutaEscritorioUsuario);    
                    }

                    if (!Directory.Exists(folderAdd))
                    {
                        _logger.LogWarning($"El directorio no existe: {folderAdd}");
                        count++;
                        _logger.LogInformation($"Ejecusion numero:{count} ");
                    }
                    else
                    {
                        AddNewShortCut(rutaEscritorioUsuario,folderAdd);
                    }
                    count++;
                    _logger.LogInformation($"Ejecusion numero:{count} ");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al limpiar accesos directos");
                }

                // Espera 10 segundos antes de volver a limpiar los accesos directos
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        public void RemoveShortCut(string rutaEscritorio)
        {
            //var files = Directory.GetFiles(item, "*.lnk", SearchOption.AllDirectories); //esta linea agrega tambien las subcarpetas del directorio
            var files = Directory.GetFiles(rutaEscritorio, "*.lnk");
            foreach (var file in files)
            {
                try
                {
                    // File.Delete(file);
                    _logger.LogInformation($"Eliminado: {file}");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error eliminando el archivo: {file}");
                }
            }           
        }
        public void AddNewShortCut(string rutaEscritorio, string rutaFolderCopiado)
        {           
            var archivos = Directory.GetFiles(rutaFolderCopiado, "*.lnk");

            foreach (var archivo in archivos)
            {
                try
                {
                    string nombreArchivo = Path.GetFileName(archivo);
                    string destino = Path.Combine(rutaEscritorio, nombreArchivo);
                    
                    //File.Copy(archivo, destino, true);

                    _logger.LogInformation($"Copiado: {archivo} -> {destino}");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error copiando el archivo: {archivo}");
                }
            }
        }        
    }
}