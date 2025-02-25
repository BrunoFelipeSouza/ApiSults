using ApiSults.Domain.Shared.Entities;

namespace ApiSults.Domain.Shared.Repositories;

public interface ILogRepository : IBaseRepository<Log>
{
    Task<IEnumerable<Log>> GetLogsBySpecification(ISpecification<Log> specification, CancellationToken cancellationToken);
    Task<Log?> GetLogBySpecification(ISpecification<Log> specification, CancellationToken cancellationToken);
}