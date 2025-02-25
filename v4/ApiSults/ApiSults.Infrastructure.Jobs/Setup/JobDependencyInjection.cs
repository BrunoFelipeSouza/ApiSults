using ApiSults.Infrastructure.Jobs.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ApiSults.Infrastructure.Jobs.Setup;

public static class JobDependencyInjection
{
    public static IServiceCollection ConfigureJobApp(this IServiceCollection services)
    {
        services.AddTransient<IApplicationServices, ApplicationServices>();
        services.AddHostedService<AutoRefreshDataJob>();
        services.AddHostedService<LogPurgeJob>();

        return services;
    }
}
