using AnimalFarm.Application.Interfaces;
using AnimalFarm.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalFarm.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IAnimalTypeService, AnimalTypeService>();
        services.AddScoped<IBreedService, BreedService>();
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<IWeightingService, WeightingService>();

        return services;
    }
}
