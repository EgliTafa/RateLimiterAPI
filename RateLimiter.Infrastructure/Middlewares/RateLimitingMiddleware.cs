using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RateLimiter.Domain.RateLimit.Interfaces;

namespace RateLimiter.Infrastructure.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;

        public RateLimitingMiddleware(ILogger<RateLimitingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, IRateLimiterService rateLimiterService)
        {
            var path = context.Request.Path.Value;

            if (path.StartsWith("/swagger") || path.StartsWith("/v1/swagger.json") || path.Contains("openapi", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (!rateLimiterService.IsRequestAllowed(ip))
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Try again later.");
                return;
            }

            await _next(context);
        }
    }
}
