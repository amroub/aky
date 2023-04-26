namespace Diatly.Foundation.CacheManager
{
    using System;
    using System.Threading.Tasks;
    using Diatly.Foundation.Utility.Serialization;
    using StackExchange.Redis;

    public class RedisCache : ICache
    {
        private readonly IDatabase database;
        private readonly ISerializer serializer;

        public RedisCache(IRedisConnectionWrapper connectionWrapper, ISerializer serializer)
        {
            this.database = connectionWrapper.Database(null);
            this.serializer = serializer;
        }

        public async Task<bool> ExistsAsync(string key) => await this.database.KeyExistsAsync(key);

        public async Task<T> GetAsync<T>(string key)
        {
            var result = await this.database.StringGetAsync(key, CommandFlags.None);

            return this.serializer.Deserialize<T>(result);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiredIn) => await this.database.StringSetAsync(key, this.serializer.Serialize(value), expiredIn);

        public async Task RemoveAsync(string key) => await this.database.KeyDeleteAsync(key);

        public bool Exists(string key) => this.database.KeyExists(key, CommandFlags.None);

        public T GetKey<T>(string key) => this.serializer.Deserialize<T>(this.database.StringGet(key, CommandFlags.PreferSlave));

        public void SetKey<T>(string key, T value, TimeSpan expiredIn) => this.database.StringSet(key, this.serializer.Serialize(value), expiredIn);

        public void Remove(string key) => this.database.KeyDelete(key);
    }
}
