using Microsoft.Extensions.DependencyInjection;

namespace ApiSults.Infrastructure.Services.Logging.Setup;

public static class LoggingDependencyInjection
{
    public static IServiceCollection ConfigureLoggingApp(this IServiceCollection services)
    {
        services.AddTransient<ILoggingService, LoggingService>();

        return services;
    }
}
