using Paytrack.Application.UseCases.Movements.Commands.CreateMovement;
using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Api.Endpoints.Movements;

internal sealed class CreateMovementEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/movements", HandleAsync)
           .WithName(nameof(CreateMovementEndpoint))
           .WithTags(Tags.Movements)
           .RequireAuthorization()
           .Produces<MovementResponse>(StatusCodes.Status201Created);
    }

    public static async Task<IResult> HandleAsync(
        IMediator mediator,
        CreateMovementCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                routeName: nameof(GetMovementByIdEndpoint),
                routeValues: new { id = result.Value.Id },
                value: result.Value)
            : CustomResults.Problem(result);
    }
}
