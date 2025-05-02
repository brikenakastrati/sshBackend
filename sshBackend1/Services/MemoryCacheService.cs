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
                value = acquire();
                _cache.Set(key, value, duration);
            }
            return value;
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> acquire, TimeSpan duration)
        {
            if (!_cache.TryGetValue(key, out T value))
            {
                value = await acquire();
                _cache.Set(key, value, duration);
            }
            return value;
        }
    }

}
