using ApiSults.Domain.Shared.Repositories;
using ApiSults.Domain.TicketsModule;
using ApiSults.Domain.TicketsModule.Repositories;
using ApiSults.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiSults.Infrastructure.Data.Repositories;

public class TicketRepository(ApiSultsContext context) : BaseRepository<Ticket>(context), ITicketRepository
{
    public async Task<IEnumerable<Ticket>> GetTicketsBySpecificationAsync(ISpecification<Ticket> specification, CancellationToken cancellationToken)
        => await DbSet
                    .Include(x => x.Supports)
                    .Include(x => x.Tags)
                    .Where(specification.ToExpression())
                    .ToListAsync(cancellationToken);

    public async Task<Ticket?> GetTicketBySpecificationAsync(ISpecification<Ticket> specification, CancellationToken cancellationToken)
        => await DbSet
                    .Include(x => x.Supports)
                    .Include(x => x.Tags)
                    .FirstOrDefaultAsync(specification.ToExpression(), cancellationToken);
}
