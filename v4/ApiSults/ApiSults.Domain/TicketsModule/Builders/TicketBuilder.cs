using ApiSults.Domain.Shared.Builders;
using ApiSults.Domain.TicketsModule.ValueObjects;

namespace ApiSults.Domain.TicketsModule;

public partial class Ticket
{
    public static Builder Create() => new();

    private readonly static HashSet<string> _mandatoryFields =
    [
        nameof(Id),
        nameof(Applicant),
        nameof(Responsible),
        nameof(Unit),
        nameof(Department),
        nameof(Subject),
        nameof(Type),
        nameof(Open),
        nameof(SolvePlanned),
        nameof(ResolveStipulated),
        nameof(LastChange),
        nameof(Status),
        nameof(CountPublicInteraction),
        nameof(CountInteractionInternal)
    ];

    public sealed class Builder : AbstractDomainBuilder<Ticket>
    {
        internal Builder() : base(new Ticket(), _mandatoryFields) { }

        public Builder WithId(long id)
        {
            MarkAsProvided(nameof(Id));
            _object.Id = id;
            return this;
        }

        public Builder WithTitle(string title)
        {
            MarkAsProvided(nameof(Title));
            _object.Title = title;
            return this;
        }

        public Builder WithApplicant(Applicant applicant)
        {
            MarkAsProvided(nameof(Applicant));
            _object.Applicant = applicant;
            return this;
        }

        public Builder WithResponsible(Responsible responsible)
        {
            MarkAsProvided(nameof(Responsible));
            _object.Responsible = responsible;
            return this;
        }

        public Builder WithUnit(Unit unit)
        {
            MarkAsProvided(nameof(Unit));
            _object.Unit = unit;
            return this;
        }

        public Builder WithDepartment(Department department)
        {
            MarkAsProvided(nameof(Department));
            _object.Department = department;
            return this;
        }

        public Builder WithSubject(Subject subject)
        {
            MarkAsProvided(nameof(Subject));
            _object.Subject = subject;
            return this;
        }

        public Builder WithSupports(List<Support>? supports)
        {
            MarkAsProvidedIf(supports != null, nameof(Supports));
            if (supports == null) return this;
            _object.Supports = supports;
            return this;
        }

        public Builder WithTags(List<Tag> tags)
        {
            MarkAsProvided(nameof(Tags));
            _object.Tags = tags;
            return this;
        }

        public Builder WithType(long type)
        {
            MarkAsProvided(nameof(Type));
            _object.Type = type;
            return this;
        }

        public Builder WithOpen(DateTime open)
        {
            MarkAsProvided(nameof(Open));
            _object.Open = open;
            return this;
        }

        public Builder WithResolved(DateTime? resolved)
        {
            MarkAsProvidedIf(resolved.HasValue, nameof(Resolved));
            _object.Resolved = resolved;
            return this;
        }

        public Builder WithCompleted(DateTime? completed)
        {
            MarkAsProvidedIf(completed.HasValue, nameof(Completed));
            _object.Completed = completed;
            return this;
        }

        public Builder WithSolvePlanned(DateTime solvePlanned)
        {
            MarkAsProvided(nameof(SolvePlanned));
            _object.SolvePlanned = solvePlanned;
            return this;
        }

        public Builder WithResolveStipulated(DateTime resolveStipulated)
        {
            MarkAsProvided(nameof(ResolveStipulated));
            _object.ResolveStipulated = resolveStipulated;
            return this;
        }

        public Builder WithFirstInteraction(DateTime? firstInteraction)
        {
            MarkAsProvided(nameof(FirstInteraction));
            _object.FirstInteraction = firstInteraction;
            return this;
        }

        public Builder WithLastChange(DateTime lastChange)
        {
            MarkAsProvided(nameof(LastChange));
            _object.LastChange = lastChange;
            return this;
        }

        public Builder WithEvaluationNote(int? evaluationNote)
        {
            MarkAsProvided(nameof(EvaluationNote));
            _object.EvaluationNote = evaluationNote;
            return this;
        }

        public Builder WithEvaluationObservation(string? evaluationObservation)
        {
            MarkAsProvided(nameof(EvaluationObservation));
            _object.EvaluationObservation = evaluationObservation;
            return this;
        }

        public Builder WithStatus(int status)
        {
            MarkAsProvided(nameof(Status));
            _object.Status = status;
            return this;
        }

        public Builder WithCountPublicInteraction(int countPublicInteraction)
        {
            MarkAsProvided(nameof(CountPublicInteraction));
            _object.CountPublicInteraction = countPublicInteraction;
            return this;
        }

        public Builder WithCountInteractionInternal(int countInteractionInternal)
        {
            MarkAsProvided(nameof(CountInteractionInternal));
            _object.CountInteractionInternal = countInteractionInternal;
            return this;
        }

        public static implicit operator Ticket(Builder builder) => builder.Build();
    }
}