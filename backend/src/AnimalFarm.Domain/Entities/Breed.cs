namespace AnimalFarm.Domain.Entities;

public class Breed
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AnimalTypeId { get; set; }
    public AnimalType AnimalType { get; set; } = null!;
    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
