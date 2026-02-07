using AnimalFarm.Application.DTOs.Animal;
using AnimalFarm.Application.Interfaces;
using AnimalFarm.Domain.Entities;
using AnimalFarm.Domain.Enums;
using AnimalFarm.Domain.Interfaces;
using FluentValidation;

namespace AnimalFarm.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _repository;
    private readonly IBreedRepository _breedRepository;
    private readonly IValidator<CreateAnimalDto> _createValidator;
    private readonly IValidator<UpdateAnimalDto> _updateValidator;

    public AnimalService(
        IAnimalRepository repository,
        IBreedRepository breedRepository,
        IValidator<CreateAnimalDto> createValidator,
        IValidator<UpdateAnimalDto> updateValidator)
    {
        _repository = repository;
        _breedRepository = breedRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<AnimalDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(MapToDto);
    }

    public async Task<AnimalDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<AnimalDto> CreateAsync(CreateAnimalDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existing = await _repository.GetByInventoryNumberAsync(dto.InventoryNumber);
        if (existing is not null)
            throw new InvalidOperationException($"Animal with inventory number '{dto.InventoryNumber}' already exists.");

        if (!await _breedRepository.ExistsAsync(dto.BreedId))
            throw new KeyNotFoundException($"Breed with id {dto.BreedId} not found.");

        if (dto.ParentAnimalId.HasValue && !await _repository.ExistsAsync(dto.ParentAnimalId.Value))
            throw new KeyNotFoundException($"Parent animal with id {dto.ParentAnimalId.Value} not found.");

        var entity = new Animal
        {
            InventoryNumber = dto.InventoryNumber,
            Gender = Enum.Parse<Gender>(dto.Gender),
            Name = dto.Name,
            ArrivalDate = dto.ArrivalDate,
            ArrivalAgeMonths = dto.ArrivalAgeMonths,
            BreedId = dto.BreedId,
            ParentAnimalId = dto.ParentAnimalId
        };

        var created = await _repository.AddAsync(entity);
        var full = await _repository.GetByIdAsync(created.Id);
        return MapToDto(full!);
    }

    public async Task<AnimalDto> UpdateAsync(int id, UpdateAnimalDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Animal with id {id} not found.");

        var existing = await _repository.GetByInventoryNumberAsync(dto.InventoryNumber);
        if (existing is not null && existing.Id != id)
            throw new InvalidOperationException($"Animal with inventory number '{dto.InventoryNumber}' already exists.");

        if (!await _breedRepository.ExistsAsync(dto.BreedId))
            throw new KeyNotFoundException($"Breed with id {dto.BreedId} not found.");

        if (dto.ParentAnimalId.HasValue && !await _repository.ExistsAsync(dto.ParentAnimalId.Value))
            throw new KeyNotFoundException($"Parent animal with id {dto.ParentAnimalId.Value} not found.");

        entity.InventoryNumber = dto.InventoryNumber;
        entity.Gender = Enum.Parse<Gender>(dto.Gender);
        entity.Name = dto.Name;
        entity.ArrivalDate = dto.ArrivalDate;
        entity.ArrivalAgeMonths = dto.ArrivalAgeMonths;
        entity.BreedId = dto.BreedId;
        entity.ParentAnimalId = dto.ParentAnimalId;

        await _repository.UpdateAsync(entity);
        var updated = await _repository.GetByIdAsync(id);
        return MapToDto(updated!);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Animal with id {id} not found.");

        await _repository.DeleteAsync(entity);
    }

    private static AnimalDto MapToDto(Animal entity)
    {
        return new AnimalDto(
            entity.Id,
            entity.InventoryNumber,
            entity.Gender.ToString(),
            entity.Name,
            entity.ArrivalDate,
            entity.ArrivalAgeMonths,
            entity.BreedId,
            entity.Breed?.Name ?? string.Empty,
            entity.ParentAnimalId,
            entity.ParentAnimal?.Name);
    }
}
