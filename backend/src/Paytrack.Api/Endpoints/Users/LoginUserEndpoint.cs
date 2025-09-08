using Paytrack.Application.UseCases.Users.Commands.LoginUser;
using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Api.Endpoints.Users;

internal sealed class LoginUserEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/users/login", HandleAsync)
           .WithName(nameof(LoginUserEndpoint))
           .WithTags(Tags.Users)
           .AllowAnonymous()
           .Produces<AuthResponse>(StatusCodes.Status200OK)
           .ProducesValidationProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> HandleAsync(
        IMediator mediator,
        LoginUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : CustomResults.Problem(result);
    }
}
