using AnimalFarm.Domain.Entities;
using AnimalFarm.Domain.Interfaces;
using AnimalFarm.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnimalFarm.Infrastructure.Repositories;

public class BreedRepository : IBreedRepository
{
    private readonly AnimalFarmDbContext _context;

    public BreedRepository(AnimalFarmDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Breed>> GetAllAsync()
    {
        return await _context.Breeds
            .Include(b => b.AnimalType)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Breed>> GetByAnimalTypeIdAsync(int animalTypeId)
    {
        return await _context.Breeds
            .Include(b => b.AnimalType)
            .Where(b => b.AnimalTypeId == animalTypeId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Breed?> GetByIdAsync(int id)
    {
        return await _context.Breeds
            .Include(b => b.AnimalType)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Breed> AddAsync(Breed entity)
    {
        _context.Breeds.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Breed entity)
    {
        _context.Breeds.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Breed entity)
    {
        _context.Breeds.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Breeds.AnyAsync(x => x.Id == id);
    }
}
