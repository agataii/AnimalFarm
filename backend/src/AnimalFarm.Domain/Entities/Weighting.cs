namespace AnimalFarm.Domain.Entities;

public class Weighting
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal WeightKg { get; set; }
}
