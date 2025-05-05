using Microsoft.Extensions.Caching.Memory;
using sshBackend1.Services.IServices;

namespace sshBackend1.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetOrAdd<T>(string key, Func<T> acquire, TimeSpan duration)
        {
            if (!_cache.TryGetValue(key, out T value))
            {
                Console.WriteLine($"[MemoryCache] MISS for key '{key}' — loading from database");
                value = acquire();
                _cache.Set(key, value, duration);
            }
            else
            {
                Console.WriteLine($"[MemoryCache] HIT for key '{key}' — returning from cache");
            }
            return value;
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> acquire, TimeSpan duration)
        {
            if (!_cache.TryGetValue(key, out T value))
            {
                Console.WriteLine($"[MemoryCache] MISS for key '{key}' — loading from database");
                value = await acquire();
                _cache.Set(key, value, duration);
            }
            else
            {
                Console.WriteLine($"[MemoryCache] HIT for key '{key}' — returning from cache");
            }
            return value;
        }
    }


}
