using AnimalFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalFarm.Infrastructure.Data.Configurations;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InventoryNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.InventoryNumber)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Gender)
            .HasMaxLength(10);

        builder.HasOne(x => x.ParentAnimal)
            .WithMany(a => a.ChildAnimals)
            .HasForeignKey(x => x.ParentAnimalId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasMany(x => x.Weightings)
            .WithOne(w => w.Animal)
            .HasForeignKey(w => w.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
