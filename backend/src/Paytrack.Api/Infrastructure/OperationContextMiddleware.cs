using Paytrack.Api.Services;
using Paytrack.Application.Common.Interfaces;

namespace Paytrack.Api.Infrastructure;

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

    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(
            CorrelationIdHeader,
            out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }

    private static bool GetIsAuthenticated(HttpContext context)
    {
        return context.User?.Identity?.IsAuthenticated == true;
    }

    private static Guid? GetUserId(HttpContext context)
    {
        var value = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var userId) ? userId : null;
    }
}