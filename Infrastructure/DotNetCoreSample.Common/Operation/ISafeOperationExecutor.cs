
namespace  AuthService.Common.Operation
{
    public interface ISafeOperationExecutor
    {
        OpResult ExecuteSafeOperation(Action action);

        OpResult<T> ExecuteSafeOperation<T>(Func<T> func, TimeSpan? timeout = null);

        Task<OpResult> ExecuteSafeOperationAsync(Func<Task> func);

        Task<OpResult<T>> ExecuteSafeOperationAsync<T>(Func<Task<T>> func, string cacheKey = null, TimeSpan? cacheDuration = null, bool refreshCache = false);
    }
}