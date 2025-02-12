using FusionCacheDemo.Api.Models;
using FusionCacheDemo.Core.Entities;
using FusionCacheDemo.Infrastructure.Caching;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FusionCacheDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly InMemoryCacheService _inMemoryCache;
    private readonly RedisCacheService _redisCache;
    private readonly HybridCacheService _hybridCache;

    public ItemController(
        InMemoryCacheService inMemoryCache,
        RedisCacheService redisCache,
        HybridCacheService hybridCache)
    {
        _inMemoryCache = inMemoryCache;
        _redisCache = redisCache;
        _hybridCache = hybridCache;
    }

    [HttpGet("inmemory")]
    public async Task<ActionResult<CacheResponse<IEnumerable<DenormalizedZipCode>>>> GetAllMemory()
    {
        //var existingMemoryData = _inMemoryCache.GetCachedDataOnlyAsync();
        var stopwatch = Stopwatch.StartNew();
        var items = await _inMemoryCache.GetAllAsync();
        stopwatch.Stop();

        if (items == null)
            return NotFound();

        return Ok(new CacheResponse<IEnumerable<DenormalizedZipCode>>
        {
            Data = items.Take(10),
            ResponseTime = stopwatch.ElapsedMilliseconds,
            CacheType = "In-Memory"
        });
    }

    [HttpGet("redis")]
    public async Task<ActionResult<CacheResponse<IEnumerable<DenormalizedZipCode>>>> GetAllRedis()
    {
        //var existingMemoryData = _inMemoryCache.GetCachedDataOnlyAsync();
        var stopwatch = Stopwatch.StartNew();
        var items = await _redisCache.GetAllAsync();
        stopwatch.Stop();

        if (items == null)
            return NotFound();

        return Ok(new CacheResponse<IEnumerable<DenormalizedZipCode>>
        {
            Data = items.Take(10),
            ResponseTime = stopwatch.ElapsedMilliseconds,
            CacheType = "Redis"
        });
    }

    [HttpGet("hybrid")]
    public async Task<ActionResult<CacheResponse<IEnumerable<DenormalizedZipCode>>>> GetAllHybrid()
    {
        //var existingMemoryData = _inMemoryCache.GetCachedDataOnlyAsync();
        var stopwatch = Stopwatch.StartNew();
        var items = await _hybridCache.GetAllAsync();
        stopwatch.Stop();

        if (items == null)
            return NotFound();

        return Ok(new CacheResponse<IEnumerable<DenormalizedZipCode>>
        {
            Data = items.Take(10),
            ResponseTime = stopwatch.ElapsedMilliseconds,
            CacheType = "Hybrid"
        });
    }

    [HttpPost("remove")]
    public async Task<IActionResult> InvalidateCache()
    {
        await _inMemoryCache.InvalidateCacheAsync();
        await _redisCache.InvalidateCacheAsync();
        await _hybridCache.InvalidateCacheAsync();
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshCache()
    {
        await _inMemoryCache.RefreshCacheAsync();
        await _redisCache.RefreshCacheAsync();
        await _hybridCache.RefreshCacheAsync();
        return Ok();
    }

    [HttpPost("benchmark")]
    public async Task<ActionResult<BenchmarkReport>> BenchmarkApi([FromBody] BenchmarkRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Url) || request.Rpm <= 0 || request.Time <= 0)
        {
            return BadRequest("Invalid input. Ensure 'url', 'rpm', and 'time' are valid.");
        }

        var responseTimes = new List<long>();
        var httpClient = new HttpClient();
        var cancellationTokenSource = new CancellationTokenSource();

        int totalRequests = request.Rpm * request.Time / 60; // Total requests based on RPM and time
        int delayBetweenRequests = 60000 / request.Rpm; // Delay between requests to achieve RPM

        var tasks = new List<Task>();

        for (int i = 0; i < totalRequests; i++)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
                break;

            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    var stopwatch = Stopwatch.StartNew();
                    var response = await httpClient.GetAsync(request.Url, cancellationTokenSource.Token);
                    stopwatch.Stop();

                    if (response.IsSuccessStatusCode)
                    {
                        responseTimes.Add(stopwatch.ElapsedMilliseconds);
                    }
                }
                catch
                {
                    // Handle errors silently for benchmarking purposes
                }
            }, cancellationTokenSource.Token));

            await Task.Delay(delayBetweenRequests, cancellationTokenSource.Token); // Maintain RPM
        }

        await Task.WhenAll(tasks);

        // Generate the report
        var report = GenerateBenchmarkReport(responseTimes);

        return Ok(report);
    }

    private BenchmarkReport GenerateBenchmarkReport(List<long> responseTimes)
    {
        if (!responseTimes.Any())
        {
            return new BenchmarkReport
            {
                TotalRequests = 0,
                SuccessfulRequests = 0,
                AverageResponseTime = 0,
                MinResponseTime = 0,
                MaxResponseTime = 0
            };
        }

        return new BenchmarkReport
        {
            TotalRequests = responseTimes.Count,
            SuccessfulRequests = responseTimes.Count,
            AverageResponseTime = responseTimes.Average(),
            MinResponseTime = responseTimes.Min(),
            MaxResponseTime = responseTimes.Max()
        };
    }
}