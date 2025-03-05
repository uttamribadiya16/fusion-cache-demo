using FusionCacheDemo.Core.Entities;
using FusionCacheDemo.Core.Interfaces;
using FusionCacheDemo.Infrastructure.Data;
using FusionCacheDemo.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace FusionCacheDemo.Infrastructure.Repositories;

public class DenormalizedZipCodeRepository : IDenormalizedZipCodeRepository
{
    private readonly ApplicationDbContext _context;

    public DenormalizedZipCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DenormalizedZipCode> GetByIdAsync(int id)
    {
        return await _context.DenormalizedZipCodes.FindAsync(id);
    }

    public async Task<IEnumerable<DenormalizedZipCode>> GetAllAsync()
    {
        return await Task.FromResult(Constants.SampleZipCodes);
        
        // Uncomment the following line to fetch data from the database
        //return await _context.DenormalizedZipCodes.ToListAsync();
    }

    public async Task<DenormalizedZipCode> AddAsync(DenormalizedZipCode zipCode)
    {
        _context.DenormalizedZipCodes.Add(zipCode);
        await _context.SaveChangesAsync();
        return zipCode;
    }

    public async Task UpdateAsync(DenormalizedZipCode zipCode)
    {
        _context.Entry(zipCode).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var zipCode = await _context.DenormalizedZipCodes.FindAsync(id);
        if (zipCode != null)
        {
            _context.DenormalizedZipCodes.Remove(zipCode);
            await _context.SaveChangesAsync();
        }
    }
}