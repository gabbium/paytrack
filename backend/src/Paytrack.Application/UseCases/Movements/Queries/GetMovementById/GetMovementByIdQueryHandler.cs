using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Domain.Repositories;
using Paytrack.Domain.Resources;

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
            return Error.NotFound(Resource.Movement_NotFound);

        return MovementResponse.FromDomain(movement);
    }
}