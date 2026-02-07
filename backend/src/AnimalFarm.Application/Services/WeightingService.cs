using AnimalFarm.Application.DTOs.Weighting;
using AnimalFarm.Application.Interfaces;
using AnimalFarm.Domain.Entities;
using AnimalFarm.Domain.Interfaces;
using FluentValidation;

namespace AnimalFarm.Application.Services;

public class WeightingService : IWeightingService
{
    private readonly IWeightingRepository _repository;
    private readonly IAnimalRepository _animalRepository;
    private readonly IUserLookupService _userLookupService;
    private readonly IValidator<CreateWeightingDto> _createValidator;
    private readonly IValidator<UpdateWeightingDto> _updateValidator;

    public WeightingService(
        IWeightingRepository repository,
        IAnimalRepository animalRepository,
        IUserLookupService userLookupService,
        IValidator<CreateWeightingDto> createValidator,
        IValidator<UpdateWeightingDto> updateValidator)
    {
        _repository = repository;
        _animalRepository = animalRepository;
        _userLookupService = userLookupService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<WeightingDto>> GetAllAsync(string userId)
    {
        var isAdmin = await _userLookupService.IsInRoleAsync(userId, "Admin");

        IEnumerable<Weighting> entities;
        if (isAdmin)
            entities = await _repository.GetAllAsync();
        else
            entities = await _repository.GetByUserIdAsync(userId);

        var dtos = new List<WeightingDto>();
        foreach (var e in entities)
            dtos.Add(await MapToDtoAsync(e));

        return dtos;
    }

    public async Task<IEnumerable<WeightingDto>> GetByUserIdAsync(string userId)
    {
        var entities = await _repository.GetByUserIdAsync(userId);
        var dtos = new List<WeightingDto>();
        foreach (var e in entities)
            dtos.Add(await MapToDtoAsync(e));
        return dtos;
    }

    public async Task<WeightingDto?> GetByIdAsync(int id, string userId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return null;

        var isAdmin = await _userLookupService.IsInRoleAsync(userId, "Admin");
        if (!isAdmin && entity.UserId != userId)
            throw new UnauthorizedAccessException("You do not have access to this weighting record.");

        return await MapToDtoAsync(entity);
    }

    public async Task<WeightingDto> CreateAsync(CreateWeightingDto dto, string userId)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (!await _animalRepository.ExistsAsync(dto.AnimalId))
            throw new KeyNotFoundException($"Animal with id {dto.AnimalId} not found.");

        if (await _repository.ExistsByAnimalAndDateAsync(dto.AnimalId, dto.Date))
            throw new InvalidOperationException(
                $"A weighting for animal {dto.AnimalId} on {dto.Date:yyyy-MM-dd} already exists.");

        var entity = new Weighting
        {
            AnimalId = dto.AnimalId,
            UserId = userId,
            Date = dto.Date,
            WeightKg = dto.WeightKg
        };

        var created = await _repository.AddAsync(entity);
        var full = await _repository.GetByIdAsync(created.Id);
        return await MapToDtoAsync(full!);
    }

    public async Task<WeightingDto> UpdateAsync(int id, UpdateWeightingDto dto, string userId)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Weighting with id {id} not found.");

        var isAdmin = await _userLookupService.IsInRoleAsync(userId, "Admin");
        if (!isAdmin && entity.UserId != userId)
            throw new UnauthorizedAccessException("You can only update your own weighting records.");

        if (!await _animalRepository.ExistsAsync(dto.AnimalId))
            throw new KeyNotFoundException($"Animal with id {dto.AnimalId} not found.");

        if (await _repository.ExistsByAnimalAndDateAsync(dto.AnimalId, dto.Date, id))
            throw new InvalidOperationException(
                $"A weighting for animal {dto.AnimalId} on {dto.Date:yyyy-MM-dd} already exists.");

        entity.AnimalId = dto.AnimalId;
        entity.Date = dto.Date;
        entity.WeightKg = dto.WeightKg;

        await _repository.UpdateAsync(entity);
        var updated = await _repository.GetByIdAsync(id);
        return await MapToDtoAsync(updated!);
    }

    public async Task DeleteAsync(int id, string userId)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Weighting with id {id} not found.");

        var isAdmin = await _userLookupService.IsInRoleAsync(userId, "Admin");
        if (!isAdmin && entity.UserId != userId)
            throw new UnauthorizedAccessException("You can only delete your own weighting records.");

        await _repository.DeleteAsync(entity);
    }

    private async Task<WeightingDto> MapToDtoAsync(Weighting entity)
    {
        var userName = await _userLookupService.GetUserNameByIdAsync(entity.UserId) ?? "Unknown";
        return new WeightingDto(
            entity.Id,
            entity.AnimalId,
            entity.Animal?.Name ?? string.Empty,
            entity.UserId,
            userName,
            entity.Date,
            entity.WeightKg);
    }
}
