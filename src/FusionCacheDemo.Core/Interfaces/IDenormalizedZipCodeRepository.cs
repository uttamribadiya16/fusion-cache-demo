using FusionCacheDemo.Core.Entities;

namespace FusionCacheDemo.Core.Interfaces;

public interface IDenormalizedZipCodeRepository
{
    Task<DenormalizedZipCode?> GetByIdAsync(int id);
    Task<IEnumerable<DenormalizedZipCode>> GetAllAsync();
    Task<DenormalizedZipCode> AddAsync(DenormalizedZipCode zipCode);
    Task UpdateAsync(DenormalizedZipCode zipCode);
    Task DeleteAsync(int id);
}