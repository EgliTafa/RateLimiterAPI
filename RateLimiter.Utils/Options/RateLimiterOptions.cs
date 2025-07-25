namespace RateLimiter.Utils.Options
{
    public class RateLimiterOptions
    {
        public const string SectionName = "RateLimitPolicy";

        public int MaxRequestsPerMinute { get; set; }
    }
}
