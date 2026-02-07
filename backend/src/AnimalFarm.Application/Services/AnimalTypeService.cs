using AnimalFarm.Application.DTOs.AnimalType;
using AnimalFarm.Application.Interfaces;
using AnimalFarm.Domain.Entities;
using AnimalFarm.Domain.Interfaces;
using FluentValidation;

namespace AnimalFarm.Application.Services;

public class AnimalTypeService : IAnimalTypeService
{
    private readonly IAnimalTypeRepository _repository;
    private readonly IValidator<CreateAnimalTypeDto> _createValidator;
    private readonly IValidator<UpdateAnimalTypeDto> _updateValidator;

    public AnimalTypeService(
        IAnimalTypeRepository repository,
        IValidator<CreateAnimalTypeDto> createValidator,
        IValidator<UpdateAnimalTypeDto> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<AnimalTypeDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => new AnimalTypeDto(e.Id, e.Name));
    }

    public async Task<AnimalTypeDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : new AnimalTypeDto(entity.Id, entity.Name);
    }

    public async Task<AnimalTypeDto> CreateAsync(CreateAnimalTypeDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing is not null)
            throw new InvalidOperationException($"Animal type with name '{dto.Name}' already exists.");

        var entity = new AnimalType { Name = dto.Name };
        var created = await _repository.AddAsync(entity);
        return new AnimalTypeDto(created.Id, created.Name);
    }

    public async Task<AnimalTypeDto> UpdateAsync(int id, UpdateAnimalTypeDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Animal type with id {id} not found.");

        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing is not null && existing.Id != id)
            throw new InvalidOperationException($"Animal type with name '{dto.Name}' already exists.");

        entity.Name = dto.Name;
        await _repository.UpdateAsync(entity);
        return new AnimalTypeDto(entity.Id, entity.Name);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Animal type with id {id} not found.");

        await _repository.DeleteAsync(entity);
    }
}
