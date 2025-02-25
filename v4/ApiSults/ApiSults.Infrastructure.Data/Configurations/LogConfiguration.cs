using ApiSults.Domain.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ApiSults.Infrastructure.Data.Configurations;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable(nameof(Log));

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("LOG_ID")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.Message)
            .HasColumnName("LOG_MESSAGE")
            .HasMaxLength(4000)
            .IsUnicode(false)
            .IsRequired();

        builder
            .Property(x => x.Level)
            .HasColumnName("LOG_LEVEL")
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired();

        builder
            .Property(x => x.Date)
            .HasColumnName("LOG_DATE")
            .IsRequired();

        builder
            .Property(x => x.Source)
            .HasColumnName("LOG_SOURCE")
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired();
    }
}
