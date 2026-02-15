using Microsoft.Extensions.Hosting;
using Serilog;

namespace NoteForge.Logging
{
    public static class LoggingExtensions
    {
        public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });

            return hostBuilder;
        }
    }
}
