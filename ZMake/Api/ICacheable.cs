namespace ZMake.Api;

public interface ICacheable
{
    /// <summary>
    /// Unique key of cacheable.
    /// </summary>
    string CacheKey { get; }
    Memory<byte> GetCache();
    bool Restore(Memory<byte> cachedData);
}
