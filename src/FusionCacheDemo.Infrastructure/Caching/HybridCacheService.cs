using FusionCacheDemo.Core.Entities;
using FusionCacheDemo.Core.Interfaces;
using FusionCacheDemo.Infrastructure.Shared;
using ZiggyCreatures.Caching.Fusion;

namespace FusionCacheDemo.Infrastructure.Caching;

public class HybridCacheService
{
    private readonly IFusionCache _cache;
    private readonly IDenormalizedZipCodeRepository _denormalizedZipCodeRepository;

    public HybridCacheService(
        IFusionCacheProvider cacheProvider,
        IDenormalizedZipCodeRepository denormalizedZipCodeRepository)
    {
        _cache = cacheProvider.GetCache("HybridCache");
        _denormalizedZipCodeRepository = denormalizedZipCodeRepository;
    }

    public async Task<DenormalizedZipCode> GetAsync(int id)
    {
        return await _cache.GetOrSetAsync<DenormalizedZipCode>(
            $"item:{id}",
            async (context) => await _denormalizedZipCodeRepository.GetByIdAsync(id),
            options => options
                .SetDuration(TimeSpan.FromDays(7))
                .SetFailSafe(true, TimeSpan.FromHours(1))
                .SetFactoryTimeouts(TimeSpan.FromSeconds(5))
        );
    }

    public async Task<IEnumerable<DenormalizedZipCode>> GetAllAsync()
    {
        return await _cache.GetOrSetAsync<IEnumerable<DenormalizedZipCode>>(
            "items:all",
            async (context) => await _denormalizedZipCodeRepository.GetAllAsync(),
            options => options
                .SetDuration(TimeSpan.FromDays(7))
                .SetFailSafe(true, TimeSpan.FromHours(1))
                .SetFactoryTimeouts(TimeSpan.FromSeconds(5))
        );
    }

    public async Task UpdateAllAsync(IEnumerable<DenormalizedZipCode> updatedItems)
    {
        // Update in-memory and distributed cache (Redis)
        await _cache.SetAsync(
            "items:all",
            Constants.UpdatedSampleZipCodes,
            options => options
                .SetDuration(TimeSpan.FromDays(7)) // Cache duration
                .SetFailSafe(true, TimeSpan.FromHours(1)) // Enable fail-safe mode
                .SetFactoryTimeouts(TimeSpan.FromSeconds(5))
        );
    }

    public async Task InvalidateCacheAsync()
    {
        await _cache.RemoveAsync("items:all");
    }

    public async Task RefreshCacheAsync()
    {
        var zipCodes = await _denormalizedZipCodeRepository.GetAllAsync();
        if (zipCodes != null)
        {
            await _cache.SetAsync($"items:all", zipCodes, options =>
                options.SetDuration(TimeSpan.FromDays(7))
                    .SetFailSafe(true, TimeSpan.FromHours(1))
                    .SetFactoryTimeouts(TimeSpan.FromSeconds(5)));
        }
    }
}