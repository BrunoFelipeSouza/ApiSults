using ApiSults.Domain.Shared.Repositories;
using System.Linq.Expressions;

namespace ApiSults.Domain.TicketsModule.Repositories;

public interface ITicketRepository : IBaseRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetTicketsBySpecificationAsync(ISpecification<Ticket> specification, CancellationToken cancellationToken);
    Task<Ticket?> GetTicketBySpecificationAsync(ISpecification<Ticket> specification, CancellationToken cancellationToken);
}
