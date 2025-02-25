using ApiSults.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSults.Presentation.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void CreateDataBase(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var serviceProvider = serviceScope.ServiceProvider;

        var apiSultsContext = serviceProvider.GetService<ApiSultsContext>()
        ?? throw new InvalidOperationException("Falha ao obter contexto para criar migrations.");

        apiSultsContext.Database.Migrate();
    }
}
