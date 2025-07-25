using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using RateLimiter.Utils.Options;
using RateLimiter.Domain.RateLimit;
using RateLimiter.Domain.RateLimit.Interfaces;

namespace RateLimiter.Application.Services
{
    public class InMemoryRateLimiterService : IRateLimiterService
    {
        private readonly ConcurrentDictionary<string, RateLimitStatus> _clients = new();
        private readonly IOptionsMonitor<RateLimiterOptions> _policy;

        public InMemoryRateLimiterService(ConcurrentDictionary<string, RateLimitStatus> clients, IOptionsMonitor<RateLimiterOptions> policy)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public bool IsRequestAllowed(string clientIp)
        {
            var now = DateTime.UtcNow;
            var policy = _policy.CurrentValue;

            var status = _clients.GetOrAdd(clientIp, _ => new RateLimitStatus());

            lock (status)
            {
                if ((now - status.WindowStart).TotalMinutes >= 1)
                {
                    status.Reset();
                    return true;
                }

                if (status.RequestCount < policy.MaxRequestsPerMinute)
                {
                    status.Increment();
                    return true;
                }

                return false;
            }
        }
    }
}
