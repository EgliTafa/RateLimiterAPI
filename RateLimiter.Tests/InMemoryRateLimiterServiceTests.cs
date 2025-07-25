using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using RateLimiter.Application.Services;
using RateLimiter.Utils.Options;
using System.Collections.Concurrent;
using RateLimiter.Domain.RateLimit;

namespace RateLimiter.Tests
{
    public class InMemoryRateLimiterServiceTests
    {
        private readonly Mock<IOptionsMonitor<RateLimiterOptions>> _mockOptions;
        private readonly ConcurrentDictionary<string, RateLimitStatus> _clients;
        private readonly InMemoryRateLimiterService _service;

        public InMemoryRateLimiterServiceTests()
        {
            _mockOptions = new Mock<IOptionsMonitor<RateLimiterOptions>>();
            _mockOptions.Setup(x => x.CurrentValue)
                        .Returns(new RateLimiterOptions { MaxRequestsPerMinute = 3 });

            _clients = new ConcurrentDictionary<string, RateLimitStatus>();
            _service = new InMemoryRateLimiterService(_clients, _mockOptions.Object);
        }

        [Fact]
        public void Allows_Request_If_Under_Limit()
        {
            var ip = "127.0.0.1";

            Assert.True(_service.IsRequestAllowed(ip));
            Assert.True(_service.IsRequestAllowed(ip));
            Assert.True(_service.IsRequestAllowed(ip));
        }

        [Fact]
        public void Denies_Request_If_Over_Limit()
        {
            var ip = "192.168.0.1";

            _service.IsRequestAllowed(ip);
            _service.IsRequestAllowed(ip); 
            _service.IsRequestAllowed(ip);
            _service.IsRequestAllowed(ip);
            var result = _service.IsRequestAllowed(ip); 

            Assert.False(result);
        }

        [Fact]
        public void Resets_Request_After_One_Minute()
        {
            var ip = "10.0.0.1";
            var status = new RateLimitStatus();

            status.Reset();
            for (int i = 0; i < 3; i++)
                status.Increment();

            typeof(RateLimitStatus)
                .GetProperty(nameof(RateLimitStatus.WindowStart))
                .SetValue(status, DateTime.UtcNow.AddMinutes(-1.1));

            _clients[ip] = status;

            var allowed = _service.IsRequestAllowed(ip);
            Assert.True(allowed);
        }
    }
}
