using Paytrack.Application.UseCases.Movements.Commands.DeleteMovement;

namespace Paytrack.Api.Endpoints.Movements;

internal sealed class DeleteMovementEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete("/movements/{id:guid}", HandleAsync)
           .WithName(nameof(DeleteMovementEndpoint))
           .WithTags(Tags.Movements)
           .RequireAuthorization()
           .Produces(StatusCodes.Status204NoContent)
           .ProducesValidationProblem(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status404NotFound);
    }
    public static async Task<IResult> HandleAsync(
        IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteMovementCommand(id);

        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : CustomResults.Problem(result);
    }
}
