using AnimalFarm.Application.DTOs.Breed;
using AnimalFarm.Application.Interfaces;
using AnimalFarm.Domain.Entities;
using AnimalFarm.Domain.Interfaces;
using FluentValidation;

namespace AnimalFarm.Application.Services;

public class BreedService : IBreedService
{
    private readonly IBreedRepository _repository;
    private readonly IAnimalTypeRepository _animalTypeRepository;
    private readonly IValidator<CreateBreedDto> _createValidator;
    private readonly IValidator<UpdateBreedDto> _updateValidator;

    public BreedService(
        IBreedRepository repository,
        IAnimalTypeRepository animalTypeRepository,
        IValidator<CreateBreedDto> createValidator,
        IValidator<UpdateBreedDto> updateValidator)
    {
        _repository = repository;
        _animalTypeRepository = animalTypeRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<BreedDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(MapToDto);
    }

    public async Task<IEnumerable<BreedDto>> GetByAnimalTypeIdAsync(int animalTypeId)
    {
        var entities = await _repository.GetByAnimalTypeIdAsync(animalTypeId);
        return entities.Select(MapToDto);
    }

    public async Task<BreedDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<BreedDto> CreateAsync(CreateBreedDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (!await _animalTypeRepository.ExistsAsync(dto.AnimalTypeId))
            throw new KeyNotFoundException($"Animal type with id {dto.AnimalTypeId} not found.");

        var entity = new Breed
        {
            Name = dto.Name,
            AnimalTypeId = dto.AnimalTypeId
        };

        var created = await _repository.AddAsync(entity);
        var full = await _repository.GetByIdAsync(created.Id);
        return MapToDto(full!);
    }

    public async Task<BreedDto> UpdateAsync(int id, UpdateBreedDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Breed with id {id} not found.");

        if (!await _animalTypeRepository.ExistsAsync(dto.AnimalTypeId))
            throw new KeyNotFoundException($"Animal type with id {dto.AnimalTypeId} not found.");

        entity.Name = dto.Name;
        entity.AnimalTypeId = dto.AnimalTypeId;
        await _repository.UpdateAsync(entity);

        var updated = await _repository.GetByIdAsync(id);
        return MapToDto(updated!);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Breed with id {id} not found.");

        await _repository.DeleteAsync(entity);
    }

    private static BreedDto MapToDto(Breed entity)
    {
        return new BreedDto(
            entity.Id,
            entity.Name,
            entity.AnimalTypeId,
            entity.AnimalType?.Name ?? string.Empty);
    }
}
