using ApiSults.Application.Shared;
using ApiSults.Application.TicketModule.Queries.ExportTickets;
using ApiSults.Application.TicketModule.Queries.GetTickets;
using ApiSults.Domain.Shared.Repositories;
using ApiSults.Domain.TicketsModule;

namespace ApiSults.Application.TicketModule.Shared;

public class TicketSpecsBuilder
{
    private long? _id;
    private long? _departmentId;
    private int? _status;
    private long? _applicant;
    private int? _type;
    private long? _responsible;
    private long? _unit;
    private long? _subject;
    private DateTime? _openStart;
    private DateTime? _openEnd;
    private DateTime? _conclusionStart;
    private DateTime? _conclusionEnd;
    private DateTime? _resolvedStart;
    private DateTime? _resolvedEnd;
    private DateTime? _lastChangeStart;
    private DateTime? _lastChangeEnd;

    public TicketSpecsBuilder WithRequest(GetTicketsRequest request)
    {
        _id = request.Id ?? _id;
        _departmentId = request.Department ?? _departmentId;
        _status = request.Status.HasValue && !request.Status.Equals(TicketStatus.Null) ? (int)request.Status.Value : _status;
        _applicant = request.Applicant ?? _applicant;
        _type = request.Type.HasValue && !request.Type.Equals(TicketType.Null) ? (int)request.Type.Value : _type;
        _responsible = request.Responsible ?? _responsible;
        _unit = request.Unit ?? _unit;
        _subject = request.Subject ?? _subject;
        _openStart = request.OpenStart ?? _openStart;
        _openEnd = request.OpenEnd ?? _openEnd;
        _conclusionStart = request.ConclusionStart ?? _conclusionStart;
        _conclusionEnd = request.ConclusionEnd ?? _conclusionEnd;
        _resolvedStart = request.ResolvedStart ?? _resolvedStart;
        _resolvedEnd = request.ResolvedEnd ?? _resolvedEnd;
        _lastChangeStart = request.LastChangeStart ?? _lastChangeStart;
        _lastChangeEnd = request.LastChangeEnd ?? _lastChangeEnd;

        return this;
    }
    public TicketSpecsBuilder WithRequest(ExportTicketsRequest request)
    {
        _id = request.Id ?? _id;
        _departmentId = request.Department ?? _departmentId;
        _status = request.Status.HasValue && !request.Status.Equals(TicketStatus.Null) ? (int)request.Status.Value : _status;
        _applicant = request.Applicant ?? _applicant;
        _type = request.Type.HasValue && !request.Type.Equals(TicketType.Null) ? (int)request.Type.Value : _type;
        _responsible = request.Responsible ?? _responsible;
        _unit = request.Unit ?? _unit;
        _subject = request.Subject ?? _subject;
        _openStart = request.OpenStart ?? _openStart;
        _openEnd = request.OpenEnd ?? _openEnd;
        _conclusionStart = request.ConclusionStart ?? _conclusionStart;
        _conclusionEnd = request.ConclusionEnd ?? _conclusionEnd;
        _resolvedStart = request.ResolvedStart ?? _resolvedStart;
        _resolvedEnd = request.ResolvedEnd ?? _resolvedEnd;
        _lastChangeStart = request.LastChangeStart ?? _lastChangeStart;
        _lastChangeEnd = request.LastChangeEnd ?? _lastChangeEnd;

        return this;
    }

    public Specification<Ticket> Build()
        => new Specification<Ticket>()
            .IsEqualIfHasValue(x => x.Id, _id)
            .IsEqualIfHasValue(x => x.Department, _departmentId)
            .IsEqualIfHasValue(x => x.Status, _status)
            .IsEqualIfHasValue(x => x.Applicant, _applicant)
            .IsEqualIfHasValue(x => x.Type, _type)
            .IsEqualIfHasValue(x => x.Responsible, _responsible)
            .IsEqualIfHasValue(x => x.Unit, _unit)
            .IsEqualIfHasValue(x => x.Subject, _subject)
            .IsBetweenIfHasValue(x => x.Open, _openStart, _openEnd)
            .IsBetweenIfHasValue(x => x.Completed, _conclusionStart, _conclusionEnd)
            .IsBetweenIfHasValue(x => x.Resolved, _resolvedStart, _resolvedEnd)
            .IsBetweenIfHasValue(x => x.LastChange, _lastChangeStart, _lastChangeEnd);

}