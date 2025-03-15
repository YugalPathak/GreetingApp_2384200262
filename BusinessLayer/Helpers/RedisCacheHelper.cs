using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace HelloGreetingApp.Helpers
{
    public class RedisCacheHelper
    {
        private readonly IDatabase _cacheDb;
        private readonly TimeSpan _cacheTimeout;

        public RedisCacheHelper(string redisConnection, int cacheTimeout)
        {
            var options = ConfigurationOptions.Parse("localhost:6379");
            options.AbortOnConnectFail = false;
            var redis = ConnectionMultiplexer.Connect(options);
            _cacheDb = redis.GetDatabase();
            _cacheTimeout = TimeSpan.FromSeconds(cacheTimeout);
        }

        public async Task SetCacheAsync<T>(string key, T value)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _cacheDb.StringSetAsync(key, jsonData, _cacheTimeout);
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var jsonData = await _cacheDb.StringGetAsync(key);
            return jsonData.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cacheDb.KeyDeleteAsync(key);
        }
    }
}