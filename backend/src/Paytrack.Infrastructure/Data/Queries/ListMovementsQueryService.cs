using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Application.UseCases.Movements.Queries.ListMovements;

namespace Paytrack.Infrastructure.Data.Queries;

internal sealed class ListMovementsQueryService(AppDbContext context) : IListMovementsQueryService
{
    public async Task<PaginatedList<MovementResponse>> ListAsync(
        ListMovementsQuery query,
        CancellationToken cancellationToken = default)
    {
        var queryable = context.Movements
            .AsNoTracking()
            .AsQueryable();

        var totalItems = await queryable.CountAsync(cancellationToken);

        var movements = await queryable
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(m => MovementResponse.FromDomain(m))
            .ToListAsync(cancellationToken);

        return new PaginatedList<MovementResponse>(movements, totalItems, query.PageNumber, query.PageSize);
    }
}
