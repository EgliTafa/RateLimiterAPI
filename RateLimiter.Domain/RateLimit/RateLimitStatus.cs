namespace RateLimiter.Domain.RateLimit
{
    public class RateLimitStatus
    {
        public int RequestCount { get; private set; }
        public DateTime WindowStart { get; private set; }

        public RateLimitStatus()
        {
            WindowStart = DateTime.UtcNow;
        }

        public void Increment()
        {
            RequestCount++;
        }

        public void Reset()
        {
            RequestCount = 1;
            WindowStart = DateTime.UtcNow;
        }
    }
}
