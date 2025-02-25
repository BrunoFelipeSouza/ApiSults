using ApiSults.Domain.ConfigurationModule;
using ApiSults.Domain.ConfigurationModule.Repositories;
using ApiSults.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSults.Infrastructure.Data.Repositories;

public class ConfigurationRepository(ApiSultsContext context) : BaseRepository<Configuration>(context), IConfigurationRepository
{
    public async Task<Configuration?> GetConfigurationAsync(CancellationToken cancellationToken)
        => await DbSet.FirstOrDefaultAsync(cancellationToken);
}
