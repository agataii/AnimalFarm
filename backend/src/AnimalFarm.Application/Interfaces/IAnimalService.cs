using AnimalFarm.Application.DTOs.Animal;

namespace AnimalFarm.Application.Interfaces;

public interface IAnimalService
{
    Task<IEnumerable<AnimalDto>> GetAllAsync();
    Task<AnimalDto?> GetByIdAsync(int id);
    Task<AnimalDto> CreateAsync(CreateAnimalDto dto);
    Task<AnimalDto> UpdateAsync(int id, UpdateAnimalDto dto);
    Task DeleteAsync(int id);
}
