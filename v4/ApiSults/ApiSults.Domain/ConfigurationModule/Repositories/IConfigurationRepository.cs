using ApiSults.Domain.Shared.Repositories;

namespace ApiSults.Domain.ConfigurationModule.Repositories;

public interface IConfigurationRepository : IBaseRepository<Configuration>
{
    Task<Configuration?> GetConfigurationAsync(CancellationToken cancellationToken); 
}
