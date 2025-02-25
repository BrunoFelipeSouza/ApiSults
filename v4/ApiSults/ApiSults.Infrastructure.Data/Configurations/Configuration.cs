using ConfigurationDomain = ApiSults.Domain.ConfigurationModule.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiSults.Infrastructure.Data.Configurations;

public class Configuration : IEntityTypeConfiguration<ConfigurationDomain>
{
    public void Configure(EntityTypeBuilder<ConfigurationDomain> builder)
    {
        builder.ToTable(nameof(Configuration).ToUpper());

        builder
            .Property(x => x.Id)
            .HasColumnName("CONFIGURATION_ID")
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(x => x.Key)
            .HasColumnName("CONFIGURATION_KEY")
            .HasMaxLength(200);

        builder
            .Property(x => x.AutomaticAtualizationIntervalInMinutes)
            .HasColumnName("CONFIGURATION_ATUALIZATION_INTERVAL")
            .HasDefaultValue(1)
            .IsRequired();

        builder
            .Property(x => x.AutomaticAtualizationEnabled)
            .HasColumnName("CONFIGURATION_ATUALIZATION_ENABLED")
            .HasDefaultValue(true)
            .IsRequired();

        builder
            .Property(x => x.LastAtualization)
            .HasColumnName("CONFIGURATION_LAST_ATUALIZATION")
            .HasDefaultValue(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Unspecified))
            .IsRequired();

        builder
            .HasData(new ConfigurationDomain()
            {
                Id = 1,
                AutomaticAtualizationIntervalInMinutes = 1,
                AutomaticAtualizationEnabled = true,
                LastAtualization = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
            });
    }
}
