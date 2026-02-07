namespace AnimalFarm.Application.DTOs.Weighting;

public record CreateWeightingDto(int AnimalId, DateTime Date, decimal WeightKg);
