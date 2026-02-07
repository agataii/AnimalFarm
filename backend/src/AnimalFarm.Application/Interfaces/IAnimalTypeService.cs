using AnimalFarm.Application.DTOs.AnimalType;

namespace AnimalFarm.Application.Interfaces;

public interface IAnimalTypeService
{
    Task<IEnumerable<AnimalTypeDto>> GetAllAsync();
    Task<AnimalTypeDto?> GetByIdAsync(int id);
    Task<AnimalTypeDto> CreateAsync(CreateAnimalTypeDto dto);
    Task<AnimalTypeDto> UpdateAsync(int id, UpdateAnimalTypeDto dto);
    Task DeleteAsync(int id);
}
