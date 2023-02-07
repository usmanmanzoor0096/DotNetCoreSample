using  AuthService.Common.Cache;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace  AuthService.Common.Operation
{
    public class DBOperationExecutor : IDBOperationExecutor
    {
        private TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(30);

        private ICache _cache;
        private readonly ILogger<DBOperationExecutor> _logger;
        public DBOperationExecutor(ICache cache, ILogger<DBOperationExecutor> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        private TransactionScope CreateTransactionScope(TimeSpan? timeout, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.Timeout = timeout ?? TimeSpan.FromSeconds(30);
            transactionOptions.IsolationLevel = isolationLevel;
            return new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);
        }

        public OpResult ExecuteOperation(Action action, TimeSpan? timeout = null, IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            try
            {
                using (var transaction = CreateTransactionScope(timeout, isolation))
                {
                    action.Invoke();
                    transaction.Complete();
                    return new OpResult(true);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.StackTrace + exception.Message);
                if (exception.InnerException != null)
                    _logger.LogError(exception.InnerException, exception.InnerException.StackTrace + exception.InnerException.Message);
                return new OpResult(exception);
            }
        }

        public OpResult<T> ExecuteOperation<T>(Func<T> func, string cacheKey = null, TimeSpan? cacheDuration = null, TimeSpan? timeout = null, IsolationLevel isolation = IsolationLevel.ReadCommitted, bool refreshCache = false)
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
                using (var transaction = CreateTransactionScope(timeout, isolation))
                {
                    var result = func.Invoke();
                    transaction.Complete();
                    if (cacheKey != null) _cache.Set(cacheKey, result, cacheDuration);
                    return new OpResult<T>(result);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.StackTrace + exception.Message);
                if (exception.InnerException != null)
                    _logger.LogError(exception.InnerException, exception.InnerException.StackTrace + exception.InnerException.Message);
                return new OpResult<T>(exception);
            }
        }

        public async Task<OpResult<T>> ExecuteOperationAsync<T>(Func<Task<T>> func, string cacheKey = null, TimeSpan? cacheDuration = null, TimeSpan? timeout = null, IsolationLevel isolation = IsolationLevel.ReadCommitted, bool refreshCache = false)
        {
            try
            {

                if (cacheKey != null)
                {
                    if (refreshCache)
                        _cache.Remove(cacheKey);
                    var result = await _cache.GetAsync<T>(cacheKey);
                    if (result != null) return new OpResult<T>(result);
                }
                using (var transaction = CreateTransactionScope(timeout, isolation))
                {
                    var result = await func.Invoke();
                    transaction.Complete();
                    if (cacheKey != null) await _cache.SetAsync(cacheKey, result, cacheDuration);
                    return new OpResult<T>(result);
                }
            }
            catch (Exception exception)
            {
                //LogExceptionXml(exception);
                _logger.LogError(exception, exception.StackTrace + exception.Message);
                if (exception.InnerException != null)
                    _logger.LogError(exception.InnerException, exception.InnerException.StackTrace + exception.InnerException.Message);
                return new OpResult<T>(exception);
            }
        }
        public async Task<OpResult> ExecuteOperationAsync(Func<Task> func, TimeSpan? timeout = null, IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            try
            {
                using (var transaction = CreateTransactionScope(timeout, isolation))
                {
                    await func.Invoke();
                    transaction.Complete();
                    return new OpResult(true);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.StackTrace + exception.Message);
                if (exception.InnerException != null)
                    _logger.LogError(exception.InnerException, exception.InnerException.StackTrace + exception.InnerException.Message);
                return new OpResult(exception);
            }
        }
    }
}