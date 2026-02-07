using AnimalFarm.Domain.Enums;

namespace AnimalFarm.Domain.Entities;

public class Animal
{
    public int Id { get; set; }
    public string InventoryNumber { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ArrivalDate { get; set; }
    public int ArrivalAgeMonths { get; set; }
    public int BreedId { get; set; }
    public Breed Breed { get; set; } = null!;
    public int? ParentAnimalId { get; set; }
    public Animal? ParentAnimal { get; set; }
    public ICollection<Animal> ChildAnimals { get; set; } = new List<Animal>();
    public ICollection<Weighting> Weightings { get; set; } = new List<Weighting>();
}
