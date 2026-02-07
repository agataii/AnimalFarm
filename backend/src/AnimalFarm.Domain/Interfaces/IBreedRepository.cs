using AnimalFarm.Domain.Entities;

namespace AnimalFarm.Domain.Interfaces;

public interface IBreedRepository
{
    Task<IEnumerable<Breed>> GetAllAsync();
    Task<IEnumerable<Breed>> GetByAnimalTypeIdAsync(int animalTypeId);
    Task<Breed?> GetByIdAsync(int id);
    Task<Breed> AddAsync(Breed entity);
    Task UpdateAsync(Breed entity);
    Task DeleteAsync(Breed entity);
    Task<bool> ExistsAsync(int id);
}
