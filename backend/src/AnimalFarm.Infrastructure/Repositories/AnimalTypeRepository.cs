using AnimalFarm.Domain.Entities;
using AnimalFarm.Domain.Interfaces;
using AnimalFarm.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnimalFarm.Infrastructure.Repositories;

public class AnimalTypeRepository : IAnimalTypeRepository
{
    private readonly AnimalFarmDbContext _context;

    public AnimalTypeRepository(AnimalFarmDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AnimalType>> GetAllAsync()
    {
        return await _context.AnimalTypes.AsNoTracking().ToListAsync();
    }

    public async Task<AnimalType?> GetByIdAsync(int id)
    {
        return await _context.AnimalTypes.FindAsync(id);
    }

    public async Task<AnimalType?> GetByNameAsync(string name)
    {
        return await _context.AnimalTypes.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<AnimalType> AddAsync(AnimalType entity)
    {
        _context.AnimalTypes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(AnimalType entity)
    {
        _context.AnimalTypes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AnimalType entity)
    {
        _context.AnimalTypes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.AnimalTypes.AnyAsync(x => x.Id == id);
    }
}
