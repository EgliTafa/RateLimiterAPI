using System.Collections.Concurrent;
using Microsoft.AspNetCore.Builder;
using RateLimiter.Application.Services;
using RateLimiter.Domain.RateLimit;
using RateLimiter.Domain.RateLimit.Interfaces;
using RateLimiter.Infrastructure.Middlewares;
using RateLimiter.Utils.Options;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register the RateLimiterOptions configuration
builder.Services.Configure<RateLimiterOptions>(
    builder.Configuration.GetSection(RateLimiterOptions.SectionName));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Rate Limiter API",
        Version = "v1"
    });
});

builder.Services.AddSingleton<ConcurrentDictionary<string, RateLimitStatus>>();
builder.Services.AddSingleton<IRateLimiterService, InMemoryRateLimiterService>();
builder.Services.AddLogging();


var app = builder.Build();
app.UseMiddleware<RateLimitingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Syllabus API v1");
    // c.RoutePrefix = string.Empty; 
});

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html", permanent: false);
    return Task.CompletedTask;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
