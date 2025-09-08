# Logging

This document describes how logging works in Paytrack.

---

## Stack

- **Serilog** configured via `appsettings*.json`.
- Sinks:
  - **Console** (developer-friendly output)
  - **Seq** (centralized log server)

---

## Configuration

Serilog is configured in `appsettings.json` (and can be overridden by environment-specific files). Example:

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.Seq"],
    "MinimumLevel": "Information",
    "Enrich": ["FromLogContext"],
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "Seq",
        "Args": { "serverUrl": "http://localhost:5341" }
      }
    ]
  }
}
```

> The Seq URL matches the docker-compose mapping (host port **5341** → container port **80**).

---

## Correlation & User Context

Requests go through a middleware that populates **CorrelationId** and **UserId** into the log context using `LogContext.PushProperty`.

```csharp
internal sealed class OperationContextMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public Task Invoke(HttpContext context, IOperationContext operationContext)
    {
        var correlationId = GetCorrelationId(context);
        var isAuthenticated = GetIsAuthenticated(context);
        var userId = GetUserId(context);

        if (operationContext is OperationContext concrete)
        {
            concrete.CorrelationId = correlationId;
            concrete.IsAuthenticated = isAuthenticated;
            concrete.UserId = userId;
        }

        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("UserId", userId))
        {
            return next.Invoke(context);
        }
    }

    // helpers omitted for brevity...
}
```

- **CorrelationId** is taken from the `X-Correlation-Id` header if present; otherwise falls back to `HttpContext.TraceIdentifier`.
- **UserId** is extracted from the authenticated principal (`ClaimTypes.NameIdentifier`) if available.

This allows filtering in Seq by:

- `CorrelationId = "..."` to trace a single request end-to-end
- `UserId = "..."` to see all logs of a user

---

## Logging Guidance

- Always prefer **structured logging** (`Log.Information("Created movement {Id}", id)`).
- Include **domain identifiers** (movement id, user id) and relevant context.
- Avoid logging secrets or sensitive payloads.
- Use **pipeline behaviors** for cross-cutting logging of commands/queries (timings and outcomes).
