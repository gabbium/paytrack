using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Application.UseCases.Movements.Queries.ListMovements;

public sealed record ListMovementsQuery(
    int PageNumber, 
    int PageSize) 
    : IQuery<PaginatedList<MovementResponse>>;
