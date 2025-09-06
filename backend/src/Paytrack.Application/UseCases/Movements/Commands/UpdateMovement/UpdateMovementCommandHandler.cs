using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Domain.Repositories;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UseCases.Movements.Commands.UpdateMovement;

internal sealed class UpdateMovementCommandHandler(
    IMovementRepository movementRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMovementCommand, MovementResponse>
{
    public async Task<Result<MovementResponse>> HandleAsync(
        UpdateMovementCommand command, 
        CancellationToken cancellationToken = default)
    {
        var movement = await movementRepository.GetByIdAsync(command.Id, cancellationToken);

        if (movement is null)
            return Error.NotFound(Resource.Movement_NotFound);

        movement.ChangeKind(command.Kind);
        movement.ChangeAmount(command.Amount);
        movement.ChangeDescription(command.Description);
        movement.ChangeOccurredOn(command.OccurredOn);

        await movementRepository.UpdateAsync(movement, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MovementResponse.FromDomain(movement);
    }
}
