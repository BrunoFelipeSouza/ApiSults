using ApiSults.Domain.TicketsModule.ValueObjects;
using ApiSults.Domain.Shared;

namespace ApiSults.Domain.TicketsModule;

public partial class Ticket : Entity
{
    public string Title { get; protected set; }
    public Applicant Applicant { get; protected set; }
    public Responsible Responsible { get; protected set; }
    public Unit Unit { get; protected set; }
    public Department Department { get; protected set; }
    public Subject Subject { get; protected set; }
    public List<Support> Supports { get; protected set; }
    public List<Tag> Tags { get; protected set; }
    public long Type { get; protected set; }
    public DateTime Open { get; protected set; }
    public DateTime? Resolved { get; protected set; }
    public DateTime? Completed { get; protected set; }
    public DateTime SolvePlanned { get; protected set; }
    public DateTime ResolveStipulated { get; protected set; }
    public DateTime? FirstInteraction { get; protected set; }
    public DateTime LastChange { get; protected set; }
    public int? EvaluationNote { get; protected set; }
    public string? EvaluationObservation { get; protected set; }
    public int Status { get; protected set; }
    public int CountPublicInteraction { get; protected set; }
    public int CountInteractionInternal { get; protected set; }

    protected Ticket() { }

    public void Update(Ticket ticket)
    {
        Title = ticket.Title;
        Applicant = ticket.Applicant;
        Responsible = ticket.Responsible;
        Unit = ticket.Unit;
        Department = ticket.Department;
        Subject = ticket.Subject;
        Supports = ticket.Supports;
        Tags = ticket.Tags;
        Type = ticket.Type;
        Open = ticket.Open;
        Resolved = ticket.Resolved;
        Completed = ticket.Completed;
        SolvePlanned = ticket.SolvePlanned;
        ResolveStipulated = ticket.ResolveStipulated;
        FirstInteraction = ticket.FirstInteraction;
        LastChange = ticket.LastChange;
        EvaluationNote = ticket.EvaluationNote;
        EvaluationObservation = ticket.EvaluationObservation;
        Status = ticket.Status;
        CountPublicInteraction = ticket.CountPublicInteraction;
        CountInteractionInternal = ticket.CountInteractionInternal;
    }
}