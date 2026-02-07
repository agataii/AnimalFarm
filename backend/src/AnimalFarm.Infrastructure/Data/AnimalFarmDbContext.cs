using AnimalFarm.Domain.Entities;
using AnimalFarm.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnimalFarm.Infrastructure.Data;

public class AnimalFarmDbContext : IdentityDbContext<ApplicationUser>
{
    public AnimalFarmDbContext(DbContextOptions<AnimalFarmDbContext> options)
        : base(options)
    {
    }

    public DbSet<AnimalType> AnimalTypes => Set<AnimalType>();
    public DbSet<Breed> Breeds => Set<Breed>();
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Weighting> Weightings => Set<Weighting>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AnimalFarmDbContext).Assembly);
    }
}
