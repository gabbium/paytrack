using Paytrack.Application.UseCases.Movements.Commands.UpdateMovement;
using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Domain.Resources;

namespace Paytrack.Api.Endpoints.Movements;

internal sealed class UpdateMovementEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPut("/movements/{id:guid}", HandleAsync)
           .WithName(nameof(UpdateMovementEndpoint))
           .WithTags(Tags.Movements)
           .RequireAuthorization()
           .Produces<MovementResponse>(StatusCodes.Status200OK)
           .ProducesValidationProblem(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        IMediator mediator,
        Guid id,
        UpdateMovementCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return CustomResults.Problem(Error.Validation(Resource.Movement_Id_NotEmpty));

        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : CustomResults.Problem(result);
    }
}
