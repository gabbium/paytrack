using Paytrack.Application.UseCases.Users.Commands.RegisterUser;

namespace Paytrack.Api.Endpoints.Users;

internal sealed class RegisterUserEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/users/register", HandleAsync)
           .AllowAnonymous()
           .WithTags(Tags.Users);
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
