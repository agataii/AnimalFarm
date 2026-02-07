using AnimalFarm.Domain.Entities;
using AnimalFarm.Domain.Interfaces;
using AnimalFarm.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnimalFarm.Infrastructure.Repositories;

public class WeightingRepository : IWeightingRepository
{
    private readonly AnimalFarmDbContext _context;

    public WeightingRepository(AnimalFarmDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Weighting>> GetAllAsync()
    {
        return await _context.Weightings
            .Include(w => w.Animal)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Weighting>> GetByUserIdAsync(string userId)
    {
        return await _context.Weightings
            .Include(w => w.Animal)
            .Where(w => w.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Weighting?> GetByIdAsync(int id)
    {
        return await _context.Weightings
            .Include(w => w.Animal)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Weighting> AddAsync(Weighting entity)
    {
        _context.Weightings.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Weighting entity)
    {
        _context.Weightings.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Weighting entity)
    {
        _context.Weightings.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Weightings.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByAnimalAndDateAsync(int animalId, DateTime date, int? excludeId = null)
    {
        var query = _context.Weightings
            .Where(w => w.AnimalId == animalId && w.Date.Date == date.Date);

        if (excludeId.HasValue)
            query = query.Where(w => w.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}
