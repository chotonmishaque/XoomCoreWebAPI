namespace XoomCore.Infrastructure.Caching;

public interface ICacheManager
{
    T? Get<T>(string key);
    void Set<T>(string key, T value, int expirationMinutes = 30);
}
