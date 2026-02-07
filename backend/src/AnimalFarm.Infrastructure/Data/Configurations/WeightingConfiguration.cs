using AnimalFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalFarm.Infrastructure.Data.Configurations;

public class WeightingConfiguration : IEntityTypeConfiguration<Weighting>
{
    public void Configure(EntityTypeBuilder<Weighting> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WeightKg)
            .HasPrecision(10, 2);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => new { x.AnimalId, x.Date })
            .IsUnique();
    }
}
