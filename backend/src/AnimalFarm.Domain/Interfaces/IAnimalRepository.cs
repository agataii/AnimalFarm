using AnimalFarm.Domain.Entities;

namespace AnimalFarm.Domain.Interfaces;

public interface IAnimalRepository
{
    Task<IEnumerable<Animal>> GetAllAsync();
    Task<Animal?> GetByIdAsync(int id);
    Task<Animal?> GetByInventoryNumberAsync(string inventoryNumber);
    Task<Animal> AddAsync(Animal entity);
    Task UpdateAsync(Animal entity);
    Task DeleteAsync(Animal entity);
    Task<bool> ExistsAsync(int id);
}
