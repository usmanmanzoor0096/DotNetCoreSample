using System;
using System.Threading.Tasks;
using System.Transactions;

namespace  AuthService.Common.Operation
{
    public interface IDBOperationExecutor
    {
        OpResult ExecuteOperation(Action action, TimeSpan? timeout = null, IsolationLevel isolation = IsolationLevel.ReadCommitted);

        OpResult<T> ExecuteOperation<T>(Func<T> func, string cacheKey = null, TimeSpan? cacheDuration = null, TimeSpan? timeout = null, IsolationLevel isolation = IsolationLevel.ReadCommitted, bool refreshCache = false);

        Task<OpResult<T>> ExecuteOperationAsync<T>(Func<Task<T>> func, string cacheKey = null, TimeSpan? cacheDuration = null, TimeSpan? timeout = null, IsolationLevel isolation = IsolationLevel.ReadCommitted, bool refreshCache = false);

        Task<OpResult> ExecuteOperationAsync(Func<Task> func, TimeSpan? timeout = null, IsolationLevel isolation = IsolationLevel.ReadCommitted);
    }
}