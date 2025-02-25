using ApiSults.Application.TicketModule.Shared;
using ApiSults.Domain.TicketsModule;
using ApiSults.Domain.TicketsModule.Repositories;
using MediatR;

namespace ApiSults.Application.TicketModule.Queries.GetTickets;

public class GetTicketsRequestHandler(ITicketRepository ticketRepository) : IRequestHandler<GetTicketsRequest, IEnumerable<Ticket>>
{
    private readonly ITicketRepository _ticketRepository = ticketRepository;

    public async Task<IEnumerable<Ticket>> Handle(GetTicketsRequest request, CancellationToken cancellationToken)
    {
        var specs = new TicketSpecsBuilder()
            .WithRequest(request)
            .Build();

        var tickets = await _ticketRepository.GetTicketsBySpecificationAsync(specs, cancellationToken);

        return tickets;
    }
}
