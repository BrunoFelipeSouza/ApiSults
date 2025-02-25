using ApiSults.Domain.ConfigurationModule.Repositories;
using ApiSults.Domain.Shared.Repositories;
using ApiSults.Domain.TicketsModule.Repositories;
using ApiSults.Infrastructure.Data.Context;
using ApiSults.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiSults.Infrastructure.Data.Setup;

public static class DataDependencyInjection
{
    public static void ConfigureDataApp(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServer");
        services.AddDbContext<ApiSultsContext>(options => options.UseSqlServer(connectionString));

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<ILogRepository, LogRepository>();
        services.AddTransient<ITicketRepository, TicketRepository>();
        services.AddTransient<IConfigurationRepository, ConfigurationRepository>();
    }
}
