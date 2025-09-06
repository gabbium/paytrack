using Paytrack.Application.UseCases.Users.Commands.LoginUser;
using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Api.Endpoints.Users;

internal sealed class LoginUserEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/users/login", HandleAsync)
           .AllowAnonymous()
           .WithTags(Tags.Users)
           .Produces<AuthResponse>(StatusCodes.Status200OK);
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
