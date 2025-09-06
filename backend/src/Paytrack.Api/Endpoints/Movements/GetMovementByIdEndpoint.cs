using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Application.UseCases.Movements.Queries.GetMovementById;

namespace Paytrack.Api.Endpoints.Movements;

internal sealed class GetMovementByIdEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/movements/{id:guid}", HandleAsync)
           .WithName(nameof(GetMovementByIdEndpoint))
           .WithTags(Tags.Movements)
           .RequireAuthorization()
           .Produces<MovementResponse>(StatusCodes.Status200OK);
    }
    public static async Task<IResult> HandleAsync(
        IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetMovementByIdQuery(id);
        
        var result = await mediator.SendAsync(query, cancellationToken);
        
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : CustomResults.Problem(result);
    }
}
