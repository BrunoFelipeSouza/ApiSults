using ApiSults.Application.Shared;
using ApiSults.Domain.TicketsModule;
using MediatR;

namespace ApiSults.Application.TicketModule.Queries.GetTickets;

public record GetTicketsRequest(
    long? Id = null,
    TicketStatus? Status = TicketStatus.Null,
    long? Applicant = null,
    TicketType? Type = TicketType.Null,
    long? Responsible = null,
    long? Unit = null,
    long? Department = null,
    long? Subject = null,
    DateTime? OpenStart = null,
    DateTime? OpenEnd = null,
    DateTime? ConclusionStart = null,
    DateTime? ConclusionEnd = null,
    DateTime? ResolvedStart = null,
    DateTime? ResolvedEnd = null,
    DateTime? LastChangeStart = null,
    DateTime? LastChangeEnd = null): IRequest<IEnumerable<Ticket>>
{
    public bool Invalid() =>
       (Id.HasValue && Id.Value <= 0) ||
       (Applicant.HasValue && Applicant.Value <= 0) ||
       (Responsible.HasValue && Responsible.Value <= 0) ||
       (Unit.HasValue && Unit.Value <= 0) ||
       (Department.HasValue && Department.Value <= 0) ||
       (Subject.HasValue && Subject.Value <= 0);
}
