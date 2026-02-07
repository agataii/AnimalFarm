namespace AnimalFarm.Application.DTOs.Weighting;

public record WeightingDto(
    int Id,
    int AnimalId,
    string AnimalName,
    string UserId,
    string UserName,
    DateTime Date,
    decimal WeightKg);
