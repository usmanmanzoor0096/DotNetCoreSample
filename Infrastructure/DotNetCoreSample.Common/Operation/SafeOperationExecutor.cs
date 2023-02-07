using  AuthService.Common.Cache;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace  AuthService.Common.Operation
{
    public class SafeOperationExecutor : ISafeOperationExecutor
    {
        private ICache _cache;
        private readonly ILogger<SafeOperationExecutor> _logger;
        public SafeOperationExecutor(ICache cache, ILogger<SafeOperationExecutor> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public OpResult ExecuteSafeOperation(Action action)
        {
            try
            {
                action.Invoke();
                return new OpResult(true);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.StackTrace + exception.Message);
                if (exception.InnerException != null)
                    _logger.LogError(exception.InnerException, exception.InnerException.StackTrace + exception.InnerException.Message);
                return new OpResult(exception);
            }
        }

        public OpResult<T> ExecuteSafeOperation<T>(Func<T> func, TimeSpan? timeout = null)
        {
            try
            {
                var result = func.Invoke();
                return new OpResult<T>(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.StackTrace + exception.Message);
                if (exception.InnerException != null)
                    _logger.LogError(exception.InnerException, exception.InnerException.StackTrace + exception.InnerException.Message);
                return new OpResult<T>(exception);
            }
        }

        public async Task<OpResult> ExecuteSafeOperationAsync(Func<Task> func)
        {
            try
            {
                await func.Invoke();
                return new OpResult(true);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.StackTrace + exception.Message);
                if (exception.InnerException != null)
                    _logger.LogError(exception.InnerException, exception.InnerException.StackTrace + exception.InnerException.Message);
                return new OpResult(exception);
            }
        }

        public async Task<OpResult<T>> ExecuteSafeOperationAsync<T>(Func<Task<T>> func, string cacheKey = null, TimeSpan? cacheDuration = null, bool refreshCache = false)
        {
            try
            {
                if (cacheKey != null)
                {
                    if (refreshCache)
                        _cache.Remove(cacheKey);
                    var result = _cache.Get<T>(cacheKey);
                    if (result != null) return new OpResult<T>(result);
                }
                var opResult = await func.Invoke();
                if (cacheKey != null) _cache.Set(cacheKey, opResult, cacheDuration);

                return new OpResult<T>(opResult);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.StackTrace + exception.Message);
                if (exception.InnerException != null)
                    _logger.LogError(exception.InnerException, exception.InnerException.StackTrace + exception.InnerException.Message);
                return new OpResult<T>(exception);
            }
        }
    }
}