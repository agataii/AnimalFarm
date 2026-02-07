namespace AnimalFarm.Application.DTOs.Animal;

public record AnimalDto(
    int Id,
    string InventoryNumber,
    string Gender,
    string Name,
    DateTime ArrivalDate,
    int ArrivalAgeMonths,
    int BreedId,
    string BreedName,
    int? ParentAnimalId,
    string? ParentAnimalName);
