namespace Paytrack.Api.Infrastructure;

internal sealed class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    public Task Invoke(HttpContext context)
    {
        var correlationId = GetCorrelationId(context);
        var userId = GetUserId(context);

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            if (!string.IsNullOrEmpty(userId))
            {
                using (LogContext.PushProperty("UserId", userId))
                {
                    return next.Invoke(context);
                }
            }

            return next.Invoke(context);
        }
    }

    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(
            CorrelationIdHeaderName,
            out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }

    private static string? GetUserId(HttpContext context)
    {
        return context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}