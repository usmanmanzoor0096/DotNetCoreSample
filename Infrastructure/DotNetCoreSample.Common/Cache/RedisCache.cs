using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AuthService.Common.Cache
{
    public class AzureRedisCache : ICache
    {
        //TODO: Update default cache time to a longer duration
        public const int DefaultCacheTimeInMinutes = 10;

        private readonly IDistributedCache _distributedCache;

        public AzureRedisCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public object Get(string key)
        {
            return _distributedCache.GetString(key);
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        public T Get<T>(string key)
        {
            var result = _distributedCache.GetString(key);
            if (result == null) return default(T);

            return JsonConvert.DeserializeObject<T>(result);
        }

        public void Invalidate(string key)
        {
            _distributedCache.Remove(key);
        }

        public bool IsSet(string key)
        {
            return _distributedCache.GetString(key) != null;
        }

        public void Set(string key, object data)
        {
            Set(key, data, TimeSpan.FromMinutes(DefaultCacheTimeInMinutes));
        }

        public void Set(string key, object data, TimeSpan? duration)
        {
            if (duration.HasValue)
            {
                var distributedCacheEntryOptions = new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.UtcNow + duration };
                _distributedCache.SetString(key, JsonConvert.SerializeObject(data), distributedCacheEntryOptions);
            }
            else
            {
                Set(key, data);
            }
        }

        public async Task<object> GetAsync(string key)
        {
            return await _distributedCache.GetAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var result = await _distributedCache.GetStringAsync(key);
            if (result == null) return default(T);

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task SetAsync(string key, object data)
        {
            await SetAsync(key, data, TimeSpan.FromMinutes(DefaultCacheTimeInMinutes));
        }

        public async Task SetAsync(string key, object data, TimeSpan? duration)
        {
            if (duration.HasValue)
            {
                var distributedCacheEntryOptions = new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.UtcNow + duration };
                await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(data), distributedCacheEntryOptions);
            }
            else
            {
                Set(key, data);
            }
        }

        public void Update(string key, object data, TimeSpan duration)
        {
            throw new NotImplementedException();
        }
    }
}
