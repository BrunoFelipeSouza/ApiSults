using ApiSults.Domain.TicketsModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiSults.Infrastructure.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable(nameof(Ticket).ToUpper());

        builder
            .Property(x => x.Id)
            .HasColumnName("TICKET_ID")
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .HasIndex(d => d.Id)
            .HasDatabaseName("TICKET_ID");

        builder
            .Property(x => x.Title)
            .HasColumnName("TICKET_TITLE")
            .HasMaxLength(2000)
            .IsUnicode(false)
            .IsRequired();

        builder
            .OwnsOne(x => x.Applicant, applicant =>
            {
                applicant
                    .Property(a => a.Id)
                    .HasColumnName("TICKET_APPLICANT_ID")
                    .IsRequired();

                applicant
                    .Property(a => a.Name)
                    .HasColumnName("TICKET_APPLICANT_NAME")
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .IsRequired();
            });

        builder
            .OwnsOne(x => x.Responsible, responsible =>
            {
                responsible
                    .Property(r => r.Id)
                    .HasColumnName("TICKET_RESPONSIBLE_ID");

                responsible
                    .Property(r => r.Name)
                    .HasColumnName("TICKET_RESPONSIBLE_NAME")
                    .HasMaxLength(2000)
                    .IsUnicode(false);
            });

        builder
            .OwnsOne(x => x.Unit, unit =>
            {
                unit
                    .Property(u => u.Id)
                    .HasColumnName("TICKET_UNIT_ID")
                    .IsRequired();

                unit
                    .Property(u => u.Name)
                    .HasColumnName("TICKET_UNIT_NAME")
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .IsRequired();
            });

        builder
            .OwnsOne(x => x.Department, department =>
            {
                department
                    .Property(d => d.Id)
                    .HasColumnName("TICKET_DEPARTMENT_ID");

                department
                    .Property(d => d.Name)
                    .HasColumnName("TICKET_DEPARTMENT_NAME")
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                department
                    .HasIndex(d => d.Id)
                    .HasDatabaseName("IX_TICKET_DEPARTMENT_ID");
            });

        builder
            .OwnsOne(x => x.Subject, subject =>
            {
                subject
                    .Property(s => s.Id)
                    .HasColumnName("TICKET_SUBJECT_ID");

                subject
                    .Property(s => s.Name)
                    .HasColumnName("TICKET_SUBJECT_NAME")
                    .HasMaxLength(2000)
                    .IsUnicode(false);
            });

        builder
            .OwnsMany(x => x.Supports, support =>
            {
                support.ToTable(nameof(support).ToUpper());

                support
                    .Property<int>("SUPPORT_ID")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SUPPORT_ID")
                    .IsRequired();

                support.HasKey("SUPPORT_ID");

                support
                    .OwnsOne(s => s.Person, person =>
                    {
                        person
                            .Property(s => s.Id)
                            .HasColumnName("SUPPORT_PERSON_ID");

                        person
                            .Property(s => s.Name)
                            .HasColumnName("SUPPORT_PERSON_NAME")
                            .HasMaxLength(2000)
                            .IsUnicode(false);
                    });

                support
                    .OwnsOne(s => s.Department, department =>
                    {
                        department
                            .Property(s => s.Id)
                            .HasColumnName("SUPPORT_DEPARTMENT_ID");

                        department
                            .Property(s => s.Name)
                            .HasColumnName("SUPPORT_DEPARTMENT_NAME")
                            .HasMaxLength(2000)
                            .IsUnicode(false);
                    });

                support
                    .Property(s => s.PersonUnit)
                    .HasColumnName("SUPPORT_PERSON_UNIT");

                support.WithOwner().HasForeignKey("TICKET_ID");
            });

        builder
            .OwnsMany(x => x.Tags, tag =>
            {
                tag.ToTable(nameof(tag).ToUpper());

                tag
                    .Property(t => t.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("TAG_ID");

                tag
                    .Property(t => t.Name)
                    .HasColumnName("TAG_NAME")
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                tag
                    .Property(t => t.Color)
                    .HasColumnName("TAG_COLOR")
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                tag.WithOwner().HasForeignKey("TICKET_ID");
            });

        builder
            .Property(x => x.Type)
            .HasColumnName("TICKET_TYPE")
            .IsRequired();

        builder
            .Property(x => x.Open)
            .HasColumnName("TICKET_OPEN")
            .IsRequired();

        builder
            .Property(x => x.Resolved)
            .HasColumnName("TICKET_RESOLVED");

        builder
            .Property(x => x.Completed)
            .HasColumnName("TICKET_COMPLETED");

        builder
            .Property(x => x.SolvePlanned)
            .HasColumnName("TICKET_SOLVE_PLANNED")
            .IsRequired();

        builder
            .Property(x => x.ResolveStipulated)
            .HasColumnName("TICKET_RESOLVE_STIPULATED")
            .IsRequired();

        builder
            .Property(x => x.FirstInteraction)
            .HasColumnName("TICKET_FIRST_INTERACTION");

        builder
            .Property(x => x.LastChange)
            .HasColumnName("TICKET_LAST_CHANGE")
            .IsRequired();

        builder
            .Property(x => x.EvaluationNote)
            .HasColumnName("TICKET_EVALUATION_NOTE");

        builder
            .Property(x => x.EvaluationObservation)
            .HasColumnName("TICKET_EVALUATION_OBSERVATION")
            .HasMaxLength(2000)
            .IsUnicode(false);

        builder
            .Property(x => x.Status)
            .HasColumnName("TICKET_STATUS")
            .IsRequired();

        builder
            .Property(x => x.CountPublicInteraction)
            .HasColumnName("TICKET_COUNT_PUBLIC_INTERACTION")
            .IsRequired();

        builder
            .Property(x => x.CountInteractionInternal)
            .HasColumnName("TICKET_COUNT_INTERACTION_INTERNAL")
            .IsRequired();
    }
}
