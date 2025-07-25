using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using RateLimiter.Domain.RateLimit;

namespace RateLimiterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RateLimitTestController : ControllerBase
    {
        private readonly ConcurrentDictionary<string, RateLimitStatus> _clients;

        public RateLimitTestController(ConcurrentDictionary<string, RateLimitStatus> clients)
        {
            _clients = clients ?? throw new ArgumentNullException(nameof(clients));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Request successful!");
        }

        [HttpPost("reset")]
        public IActionResult Reset([FromQuery] string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return BadRequest("IP address is required.");

            bool removed = _clients.TryRemove(ip, out _);

            return removed
                ? Ok($"Rate limit reset for IP: {ip}")
                : NotFound($"No rate limit found for IP: {ip}");
        }
    }
}
