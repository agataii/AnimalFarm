using AnimalFarm.Domain.Entities;
using AnimalFarm.Domain.Interfaces;
using AnimalFarm.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnimalFarm.Infrastructure.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly AnimalFarmDbContext _context;

    public AnimalRepository(AnimalFarmDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Animal>> GetAllAsync()
    {
        return await _context.Animals
            .Include(a => a.Breed)
            .Include(a => a.ParentAnimal)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Animal?> GetByIdAsync(int id)
    {
        return await _context.Animals
            .Include(a => a.Breed)
            .Include(a => a.ParentAnimal)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Animal?> GetByInventoryNumberAsync(string inventoryNumber)
    {
        return await _context.Animals
            .FirstOrDefaultAsync(a => a.InventoryNumber == inventoryNumber);
    }

    public async Task<Animal> AddAsync(Animal entity)
    {
        _context.Animals.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Animal entity)
    {
        _context.Animals.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Animal entity)
    {
        _context.Animals.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Animals.AnyAsync(x => x.Id == id);
    }
}
