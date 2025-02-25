using ApiSults.Domain.Shared.Repositories;
using ApiSults.Infrastructure.Data.Context;

namespace ApiSults.Infrastructure.Data.Repositories;

public class UnitOfWork(ApiSultsContext apiSultsContext) : IUnitOfWork
{
    private readonly ApiSultsContext _apiSultsContext = apiSultsContext;

    public async Task CommitAsync(CancellationToken cancellationToken)
        => await _apiSultsContext.SaveChangesAsync(cancellationToken);
}
