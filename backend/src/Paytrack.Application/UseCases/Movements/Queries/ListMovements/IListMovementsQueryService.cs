using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Application.UseCases.Movements.Queries.ListMovements;

public interface IListMovementsQueryService
{
    Task<PaginatedList<MovementResponse>> ListAsync(
        ListMovementsQuery query, 
        CancellationToken cancellationToken = default);
}
