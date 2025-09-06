using Paytrack.Domain.Events;
using Paytrack.Domain.Repositories;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UseCases.Movements.Commands.DeleteMovement;

internal sealed class DeleteMovementCommandHandler(
    IMovementRepository movementRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteMovementCommand>
{
    public async Task<Result> HandleAsync(
        DeleteMovementCommand command, 
        CancellationToken cancellationToken = default)
    {
        var movement = await movementRepository.GetByIdAsync(command.Id, cancellationToken);

        if (movement is null)
            return Error.NotFound(Resource.Movement_NotFound);

        movement.AddDomainEvent(MovementDeletedEvent.FromDomain(movement));

        await movementRepository.RemoveAsync(movement, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
