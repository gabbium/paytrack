using Paytrack.Application.UseCases.Movements.Commands.UpdateMovement;
using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Api.Endpoints.Movements;

internal sealed class UpdateMovementEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPut("/movements/{id:guid}", HandleAsync)
           .WithName(nameof(UpdateMovementEndpoint))
           .WithTags(Tags.Movements)
           .RequireAuthorization()
           .Produces<MovementResponse>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(
        IMediator mediator,
        Guid id,
        UpdateMovementCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id) 
            return TypedResults.BadRequest();

        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : CustomResults.Problem(result);
    }
}
