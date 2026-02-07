using AnimalFarm.Application.Interfaces;
using AnimalFarm.Domain.Interfaces;
using AnimalFarm.Infrastructure.Data;
using AnimalFarm.Infrastructure.Identity;
using AnimalFarm.Infrastructure.Repositories;
using AnimalFarm.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalFarm.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AnimalFarmDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AnimalFarmDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IAnimalTypeRepository, AnimalTypeRepository>();
        services.AddScoped<IBreedRepository, BreedRepository>();
        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<IWeightingRepository, WeightingRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserLookupService, UserLookupService>();
        services.AddScoped<IEmailService, ConsoleEmailService>();

        return services;
    }
}
