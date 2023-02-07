using System;
using System.Threading.Tasks;

namespace  AuthService.Common.Cache
{
    public interface ICache
    {
        object Get(string key);

        T Get<T>(string key);

        Task<object> GetAsync(string key);

        Task<T> GetAsync<T>(string key);

        void Set(string key, object data);

        void Set(string key, object data, TimeSpan? duration);

        Task SetAsync(string key, object data);

        Task SetAsync(string key, object data, TimeSpan? duration);

        bool IsSet(string key);

        void Invalidate(string key);

        void Update(string key, object data, TimeSpan duration);

        void Remove(string key);
    }
}