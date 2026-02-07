using AnimalFarm.Application.DTOs.Breed;

namespace AnimalFarm.Application.Interfaces;

public interface IBreedService
{
    Task<IEnumerable<BreedDto>> GetAllAsync();
    Task<IEnumerable<BreedDto>> GetByAnimalTypeIdAsync(int animalTypeId);
    Task<BreedDto?> GetByIdAsync(int id);
    Task<BreedDto> CreateAsync(CreateBreedDto dto);
    Task<BreedDto> UpdateAsync(int id, UpdateBreedDto dto);
    Task DeleteAsync(int id);
}
