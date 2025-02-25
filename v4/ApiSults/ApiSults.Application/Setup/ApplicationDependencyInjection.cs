using ApiSults.Application.Shared.DataBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ApiSults.Application.Setup;

public static class ApplicationDependencyInjection
{
    public static void ConfigureApplicationApp(this IServiceCollection services,
                                               IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        var connectionString = configuration.GetConnectionString("SqlServer")
        ?? throw new InvalidOperationException("A string de conexão 'SqlServer' não foi encontrada no arquivo de configuração.");

        services.AddTransient<IDbConnectionFactory>(provider => new DbConnectionFactory(connectionString));
    }
}
