namespace AnimalFarm.Application.DTOs.Animal;

public record UpdateAnimalDto(
    string InventoryNumber,
    string Gender,
    string Name,
    DateTime ArrivalDate,
    int ArrivalAgeMonths,
    int BreedId,
    int? ParentAnimalId);
