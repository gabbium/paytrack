using Paytrack.Domain.Exceptions;

namespace Paytrack.Api.Infrastructure;

internal sealed class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, CancellationToken, Task>> _exceptionHandlers = new()
    {
        { typeof(BadHttpRequestException), HandleBadHttpRequestException },
        { typeof(DomainRuleViolation), HandleDomainRuleViolationException }
    };

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.TryGetValue(
            exceptionType,
            out Func<HttpContext, Exception, CancellationToken, Task>? value))
        {
            await value.Invoke(httpContext, exception, cancellationToken);
            return true;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = "Server failure",
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static async Task HandleBadHttpRequestException(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var ex = (BadHttpRequestException)exception;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Bad Request",
            Detail = ex.InnerException?.Message ?? ex.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private static async Task HandleDomainRuleViolationException(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var ex = (DomainRuleViolation)exception;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = ex.Code,
            Detail = ex.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }
}
