using Microsoft.Extensions.DependencyInjection;

namespace ApiSults.Infrastructure.Services.RefreshData.Setup;

public static class DataRefreshDependencyInjection
{
    public static IServiceCollection ConfigureRefreshDataApp(this IServiceCollection services)
    {
        services.AddTransient<IRefreshDataService, RefreshDataService>();

        return services;
    }
}
