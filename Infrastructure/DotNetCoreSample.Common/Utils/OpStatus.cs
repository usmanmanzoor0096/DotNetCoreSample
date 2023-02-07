namespace  AuthService.Common.Utils
{
    public class OpStatus
    {

        private int _maxRetryCount;
        private TimeSpan _timeSpanBetweenRetries;
        private TimeSpan _maxTimeSpanBetweenRetries;
        private double _retryTimeSpanGrowth;

        public bool Succeeded { get; set; }
        public bool Failed { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetryCount { get; set; }

        public TimeSpan TimeSpanBetweenRetries { get; set; }
        public TimeSpan MaxTimeSpanBetweenRetries { get; set; }

        public double RetryTimeSpanGrowth { get; set; }

        public DateTime LastRetryDate { get; set; }
        public Exception Exception { get; set; }

        //Default retry values
        public OpStatus()
        {
            _maxRetryCount = MaxRetryCount = 5;
            _timeSpanBetweenRetries = TimeSpanBetweenRetries = TimeSpan.FromSeconds(1);
            _maxTimeSpanBetweenRetries = MaxTimeSpanBetweenRetries = TimeSpan.FromSeconds(60);
            _retryTimeSpanGrowth = RetryTimeSpanGrowth = 2;
        }

        public void Reset()
        {
            MaxRetryCount = _maxRetryCount;
            TimeSpanBetweenRetries = _timeSpanBetweenRetries;
            MaxTimeSpanBetweenRetries = _maxTimeSpanBetweenRetries;
            RetryTimeSpanGrowth = _retryTimeSpanGrowth;
        }

        public OpStatus(int maxRetryCount, TimeSpan timeSpanBetweenRetries, TimeSpan maxTimeSpanBetweenRetries, double retryTimeSpanGrowth)
        {
            _maxRetryCount = MaxRetryCount = maxRetryCount;
            _timeSpanBetweenRetries = TimeSpanBetweenRetries = timeSpanBetweenRetries;
            _maxTimeSpanBetweenRetries = MaxTimeSpanBetweenRetries = maxTimeSpanBetweenRetries;
            _retryTimeSpanGrowth = RetryTimeSpanGrowth = retryTimeSpanGrowth;
        }

        //Returns true if MaxRetryCount has been reached
        public async Task<(bool maxReached, TimeSpan timeSpanUntilNextRetry)> CountOperationFailedAndWaitRetryDelay(Exception ex = null)
        {
            if (RetryCount + 1 > MaxRetryCount)
            {
                Failed = true;
                return (true, default(TimeSpan));
            }
            await Task.Delay(TimeSpanBetweenRetries);
            RetryCount++;
            //TimeSpanBetweenRetries = TimeSpanBetweenRetries * RetryTimeSpanGrowth < MaxTimeSpanBetweenRetries ? TimeSpanBetweenRetries * RetryTimeSpanGrowth : MaxTimeSpanBetweenRetries;
            LastRetryDate = DateTime.UtcNow;
            Exception = ex;
            return (false, TimeSpanBetweenRetries);
        }
    }
}
