using ApiSults.Domain.Shared.Entities;
using ApiSults.Domain.Shared.Repositories;
using ApiSults.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiSults.Infrastructure.Data.Repositories;

public class LogRepository(ApiSultsContext context) : BaseRepository<Log>(context), ILogRepository
{
    public async Task<Log?> GetLogBySpecification(ISpecification<Log> specification, CancellationToken cancellationToken)
        => await DbSet.FirstOrDefaultAsync(specification.ToExpression(), cancellationToken);

    public async Task<IEnumerable<Log>> GetLogsBySpecification(ISpecification<Log> specification, CancellationToken cancellationToken)
        => await DbSet
                    .Where(specification.ToExpression())
                    .ToListAsync(cancellationToken);
}
