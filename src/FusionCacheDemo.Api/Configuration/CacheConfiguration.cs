using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace FusionCacheDemo.Api.Configuration
{
    public static class CacheConfiguration
    {
        public static IServiceCollection AddCachingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Redis Cache
            ConfigureRedis(services, configuration);

            // Configure different cache setups
            ConfigureInMemoryCache(services);
            ConfigureRedisCache(services);
            ConfigureHybridCache(services);

            return services;
        }

        /// <summary>
        /// Registers Redis as the IDistributedCache backend.
        /// </summary>
        private static void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                var redisConnectionString = configuration.GetConnectionString("Redis");
                if (string.IsNullOrWhiteSpace(redisConnectionString))
                {
                    throw new InvalidOperationException("Redis connection string is missing. Please check the configuration.");
                }

                options.Configuration = redisConnectionString;
                options.InstanceName = "FusionCacheDemo:"; // Prefix for cache keys
            });

            // Debugging log
            Console.WriteLine("Redis configuration applied successfully.");
        }

        /// <summary>
        /// Configures FusionCache with In-Memory caching only.
        /// </summary>
        private static void ConfigureInMemoryCache(IServiceCollection services)
        {
            services.AddFusionCache("InMemoryCache")
                .WithOptions(opt =>
                {
                    opt.DefaultEntryOptions = new FusionCacheEntryOptions
                    {
                        Duration = TimeSpan.FromMinutes(10), // Default TTL
                        IsFailSafeEnabled = true,           // Enable fail-safe
                        FailSafeThrottleDuration = TimeSpan.FromSeconds(5),
                    };
                })
                .WithMemoryCache(provider => provider.GetRequiredService<IMemoryCache>()) // In-memory backend
                .WithSystemTextJsonSerializer();

            Console.WriteLine("FusionCache In-Memory configuration applied.");
        }

        /// <summary>
        /// Configures FusionCache with Redis as the cache backend.
        /// </summary>
        private static void ConfigureRedisCache(IServiceCollection services)
        {
            services.AddFusionCache("RedisCache")
                .WithOptions(opt =>
                {
                   // opt.CacheKeyPrefix = "FusionCacheDemo:v1:";
                    opt.DefaultEntryOptions = new FusionCacheEntryOptions
                    {
                        Duration = TimeSpan.FromMinutes(10),
                        IsFailSafeEnabled = true,
                        FailSafeThrottleDuration = TimeSpan.FromSeconds(5),
                    };
                })
                .WithDistributedCache(provider => provider.GetRequiredService<IDistributedCache>()) // Redis backend
                .WithSystemTextJsonSerializer();

            Console.WriteLine("FusionCache Redis-only configuration applied.");
        }

        /// <summary>
        /// Configures FusionCache with Hybrid (In-Memory + Redis) caching.
        /// </summary>
        private static void ConfigureHybridCache(IServiceCollection services)
        {
            services.AddFusionCache("HybridCache")
                .WithOptions(opt =>
                {
                    opt.DefaultEntryOptions = new FusionCacheEntryOptions
                    {
                        Duration = TimeSpan.FromMinutes(10), // Default TTL
                        IsFailSafeEnabled = true,           // Enable fail-safe
                        FailSafeThrottleDuration = TimeSpan.FromSeconds(5),
                    };
                })
                .WithMemoryCache(provider => provider.GetRequiredService<IMemoryCache>()) // In-Memory backend
                .WithDistributedCache(provider => provider.GetRequiredService<IDistributedCache>()) // Redis backend
                .WithSystemTextJsonSerializer();

            Console.WriteLine("FusionCache Hybrid configuration applied.");
        }
    }
}