using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Application.UseCases.Movements.Queries.GetMovementById;

public sealed record GetMovementByIdQuery(
    Guid Id) 
    : IQuery<MovementResponse>;
