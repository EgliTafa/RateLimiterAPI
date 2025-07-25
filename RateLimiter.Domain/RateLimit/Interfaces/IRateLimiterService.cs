namespace RateLimiter.Domain.RateLimit.Interfaces
{
    public interface IRateLimiterService
    {
        bool IsRequestAllowed(string clientIp);
    }
}
