using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Application.UseCases.Movements.Queries.ListMovements;

namespace Paytrack.Api.Endpoints.Movements;

internal sealed class ListMovementsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/movements", HandleAsync)
           .WithName(nameof(ListMovementsEndpoint))
           .WithTags(Tags.Movements)
           .RequireAuthorization()
           .Produces<PaginatedList<MovementResponse>>(StatusCodes.Status200OK);
    }
    public static async Task<IResult> HandleAsync(
        IMediator mediator,
        [AsParameters] ListMovementsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : CustomResults.Problem(result);
    }
}