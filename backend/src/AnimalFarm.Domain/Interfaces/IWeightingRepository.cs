using AnimalFarm.Domain.Entities;

namespace AnimalFarm.Domain.Interfaces;

public interface IWeightingRepository
{
    Task<IEnumerable<Weighting>> GetAllAsync();
    Task<IEnumerable<Weighting>> GetByUserIdAsync(string userId);
    Task<Weighting?> GetByIdAsync(int id);
    Task<Weighting> AddAsync(Weighting entity);
    Task UpdateAsync(Weighting entity);
    Task DeleteAsync(Weighting entity);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByAnimalAndDateAsync(int animalId, DateTime date, int? excludeId = null);
}
