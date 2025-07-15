using Serilog.Events;
using Serilog;

namespace E_Agenda.WebApp.DependencyInjection
{
    public static class SerilogConfig
    {

        public static void AddSerilogConfig(this IServiceCollection services, ILoggingBuilder logging) 
        {
            var caminhoAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var caminhoArquivoLogs = Path.Combine(caminhoAppData, "E-Agenda", "logs.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(caminhoArquivoLogs, LogEventLevel.Error)
                .CreateLogger();

            logging.ClearProviders();
            services.AddSerilog();
        }
    }
}
