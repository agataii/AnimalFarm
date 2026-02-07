using AnimalFarm.Application.DTOs.Weighting;

namespace AnimalFarm.Application.Interfaces;

public interface IWeightingService
{
    Task<IEnumerable<WeightingDto>> GetAllAsync(string userId);
    Task<IEnumerable<WeightingDto>> GetByUserIdAsync(string userId);
    Task<WeightingDto?> GetByIdAsync(int id, string userId);
    Task<WeightingDto> CreateAsync(CreateWeightingDto dto, string userId);
    Task<WeightingDto> UpdateAsync(int id, UpdateWeightingDto dto, string userId);
    Task DeleteAsync(int id, string userId);
}
