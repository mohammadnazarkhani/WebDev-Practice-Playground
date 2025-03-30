using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EFCoreShowcase.Common.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _cache.GetStringAsync(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
    {
        var result = new Dictionary<string, T>();
        foreach (var key in keys)
        {
            var value = await GetAsync<T>(key);
            if (value != null)
            {
                result[key] = value;
            }
        }
        return result;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(60)
        };

        var jsonValue = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, jsonValue, options);
    }

    public async Task SetAllAsync<T>(IDictionary<string, T> keyValues, TimeSpan? expiration = null)
    {
        foreach (var kvp in keyValues)
        {
            await SetAsync(kvp.Key, kvp.Value, expiration);
        }
    }

    public Task RemoveAsync(string key)
        => _cache.RemoveAsync(key);

    public Task RemoveByPrefixAsync(string prefixKey)
    {
        // Throw synchronously since this is not implemented
        throw new NotImplementedException("Prefix-based removal requires additional Redis features implementation");
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var value = await _cache.GetAsync(key);
        return value != null;
    }
}
