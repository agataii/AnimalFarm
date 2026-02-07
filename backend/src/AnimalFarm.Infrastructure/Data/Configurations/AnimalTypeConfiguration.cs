using AnimalFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalFarm.Infrastructure.Data.Configurations;

public class AnimalTypeConfiguration : IEntityTypeConfiguration<AnimalType>
{
    public void Configure(EntityTypeBuilder<AnimalType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasMany(x => x.Breeds)
            .WithOne(b => b.AnimalType)
            .HasForeignKey(b => b.AnimalTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
