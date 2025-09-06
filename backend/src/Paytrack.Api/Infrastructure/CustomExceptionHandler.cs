namespace Paytrack.Api.Infrastructure;

internal sealed class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        Error error = exception switch
        {
            DomainException de => de.Error,
            _ => Error.Failure("An unexpected error occurred"),
        };

        var result = CustomResults.Problem(error);

        await result.ExecuteAsync(httpContext);

        return true;
    }
}
