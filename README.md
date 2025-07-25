
# 📊 RateLimiterAPI

A minimal, clean, and configurable **Rate Limiting Middleware** implementation in ASP.NET Core using Domain-Driven Design (DDD).

---

## 🚀 Features

- ✅ IP-based rate limiting
- ✅ Configurable requests per minute (via `appsettings.json`)
- ✅ Dynamic configuration reload using `IOptionsMonitor`
- ✅ Middleware-based enforcement
- ✅ Swagger UI for API documentation
- ✅ In-memory tracking using `ConcurrentDictionary`
- ✅ Controller endpoint to manually reset request counters
- ✅ Unit tests using xUnit + Moq
- ✅ Modular service registration

---

## 🔧 Technologies Used

| Area        | Tech                         |
|-------------|------------------------------|
| Framework   | .NET 9                       |
| Architecture| Domain-Driven Design (DDD)   |
| Rate Limiting | Custom Middleware + InMemory |
| Testing     | xUnit, Moq                   |
| API Docs    | Swagger / Swashbuckle        |
| Config      | IOptionsMonitor<T>           |
| Logging     | Microsoft.Extensions.Logging |

---

## 📂 Project Structure

```
RateLimiterAPI/
├── Application/
│   └── Services/                   # Core services (e.g., InMemoryRateLimiterService)
├── Domain/
│   └── RateLimit/                 # Entities, Interfaces, Models
├── Infrastructure/
│   └── Middlewares/              # RateLimitingMiddleware
├── Utils/
│   └── Options/RateLimiterOptions.cs
├── Controllers/
│   └── RateLimitTestController.cs
├── Program.cs
├── appsettings.json
└── README.md
```

---

## 🧪 Running Unit Tests

### Prerequisites
- .NET 9 SDK
- `Moq` and `xUnit`

### Run all tests:
```bash
dotnet test
```

> ✅ Tests cover:
> - Accepting requests below the limit
> - Blocking requests over the limit
> - Resetting counts after the time window

---

## 🛠 Configuration

```json
"RateLimitPolicy": {
  "MaxRequestsPerMinute": 5
}
```

Set in `appsettings.json`. The value can be changed at runtime and is picked up immediately.

---

## 📘 Swagger Access

### Launch Application
```bash
dotnet run
```

### Access Swagger UI:

```
https://localhost:7000/swagger/index.html
```

or simply:

```
https://localhost:7000/
```

(automatic redirect to Swagger UI is enabled)

---

## 🔁 Reset Rate Limit for an IP

### Endpoint:
```http
POST /api/ratelimittest/reset?ip=127.0.0.1
```

This clears the IP from the in-memory dictionary and allows it to start fresh.

---

## 🔒 Notes

- Swagger routes (`/swagger`) are **excluded from rate limiting**.
- Requests from each unique IP are tracked independently.

---

## 🤝 Contributing

Pull requests and feedback are welcome! This is a lightweight and extensible base for more advanced rate limiting strategies (e.g. token bucket, Redis-based distributed limiter, etc.).

---

## 📄 License

MIT – free to use and modify.
