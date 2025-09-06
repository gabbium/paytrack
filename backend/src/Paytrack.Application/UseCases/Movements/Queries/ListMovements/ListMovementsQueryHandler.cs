using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Application.UseCases.Movements.Queries.ListMovements;

internal sealed class ListMovementsQueryHandler(
    IListMovementsQueryService listMovementsQueryService) 
    : IQueryHandler<ListMovementsQuery, PaginatedList<MovementResponse>>
{
    public async Task<Result<PaginatedList<MovementResponse>>> HandleAsync(
        ListMovementsQuery request, 
        CancellationToken cancellationToken = default)
    {
        return await listMovementsQueryService.ListAsync(request, cancellationToken);
    }
}