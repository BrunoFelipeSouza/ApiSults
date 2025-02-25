using Microsoft.EntityFrameworkCore;

namespace ApiSults.Infrastructure.Data.Context;

public class ApiSultsContext(DbContextOptions<ApiSultsContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiSultsContext).Assembly);
    }
}
