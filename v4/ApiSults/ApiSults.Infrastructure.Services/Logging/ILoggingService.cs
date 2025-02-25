using ApiSults.Domain.Shared.Entities;
using ApiSults.Domain.Shared.Repositories;

namespace ApiSults.Infrastructure.Services.Logging;

public interface ILoggingService
{
    Task LogInformation(string source, string message); 
    Task LogError(string source, string message);
    Task LogWarning(string source, string message);
}
