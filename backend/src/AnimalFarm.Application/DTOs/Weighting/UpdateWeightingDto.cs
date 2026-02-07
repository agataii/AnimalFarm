namespace AnimalFarm.Application.DTOs.Weighting;

public record UpdateWeightingDto(int AnimalId, DateTime Date, decimal WeightKg);
