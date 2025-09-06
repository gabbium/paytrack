using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Domain.Errors;
using Paytrack.Domain.Repositories;

namespace Paytrack.Application.UseCases.Movements.Queries.GetMovementById;

internal sealed class GetMovementByIdQueryHandler(
    IMovementRepository movementRepository) 
    : IQueryHandler<GetMovementByIdQuery, MovementResponse>
{
    public async Task<Result<MovementResponse>> HandleAsync(
        GetMovementByIdQuery query, 
        CancellationToken cancellationToken = default)
    {
        var movement = await movementRepository.GetByIdAsync(query.Id, cancellationToken);

        if (movement is null)
            return MovementErrors.NotFound;

        return MovementResponse.FromDomain(movement);
    }
}