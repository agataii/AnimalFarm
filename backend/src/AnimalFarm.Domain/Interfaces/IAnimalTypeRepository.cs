using AnimalFarm.Domain.Entities;

namespace AnimalFarm.Domain.Interfaces;

public interface IAnimalTypeRepository
{
    Task<IEnumerable<AnimalType>> GetAllAsync();
    Task<AnimalType?> GetByIdAsync(int id);
    Task<AnimalType?> GetByNameAsync(string name);
    Task<AnimalType> AddAsync(AnimalType entity);
    Task UpdateAsync(AnimalType entity);
    Task DeleteAsync(AnimalType entity);
    Task<bool> ExistsAsync(int id);
}
