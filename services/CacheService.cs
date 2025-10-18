using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace TeamSyncB.services
{
    public class CacheService
    {
        private readonly IDistributedCache _cache;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var cached = await _cache.GetStringAsync(key);
            if (cached == null) return null;
            Console.WriteLine($"Retrieved from cache: {key}");
            return JsonSerializer.Deserialize<T>(cached);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? _cacheExpiry
            };
            
            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, options);
            Console.WriteLine($"Added to cache: {key}");
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
            Console.WriteLine($"Removed from cache: {key}");
        }
    }
}

