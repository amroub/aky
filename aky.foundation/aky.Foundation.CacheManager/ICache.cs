namespace Diatly.Foundation.CacheManager
{
    using System;
    using System.Threading.Tasks;

    public interface ICache
    {
        bool Exists(string key);

        T GetKey<T>(string key);

        void SetKey<T>(string key, T value, TimeSpan expiredIn);

        void Remove(string key);

        Task<bool> ExistsAsync(string key);

        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T value, TimeSpan expiredIn);

        Task RemoveAsync(string key);
    }
}
