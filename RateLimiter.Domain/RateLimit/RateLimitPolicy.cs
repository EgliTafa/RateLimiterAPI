namespace RateLimiter.Domain.RateLimit
{
    public class RateLimitPolicy
    {
        public int MaxRequestsPerMinute { get; set; }

        public RateLimitPolicy() { }

        public RateLimitPolicy(int maxRequestsPerMinute)
        {
            MaxRequestsPerMinute = maxRequestsPerMinute;
        }
    }
}
