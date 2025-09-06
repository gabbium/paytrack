using Paytrack.Application.UseCases.Users.Commands.RegisterUser;
using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Api.Endpoints.Users;

internal sealed class RegisterUserEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/users/register", HandleAsync)
           .WithName(nameof(RegisterUserEndpoint))
           .WithTags(Tags.Users)
           .AllowAnonymous()
           .Produces<AuthResponse>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(
        IMediator mediator,
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);
        
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : CustomResults.Problem(result);
    }
}
