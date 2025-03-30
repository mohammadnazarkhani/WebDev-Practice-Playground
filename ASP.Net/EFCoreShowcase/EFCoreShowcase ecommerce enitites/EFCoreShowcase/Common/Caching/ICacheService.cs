namespace EFCoreShowcase.Common.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task RemoveByPrefixAsync(string prefixKey);
    Task<bool> ExistsAsync(string key);
    Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys);
    Task SetAllAsync<T>(IDictionary<string, T> keyValues, TimeSpan? expiration = null);
}
