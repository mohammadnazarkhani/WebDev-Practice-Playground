namespace EFCoreShowcase.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CacheableAttribute : Attribute
{
    public int Duration { get; }
    public string? CacheKeyPrefix { get; }

    public CacheableAttribute(int duration = 60, string? cacheKeyPrefix = null)
    {
        Duration = duration;
        CacheKeyPrefix = cacheKeyPrefix;
    }
}
