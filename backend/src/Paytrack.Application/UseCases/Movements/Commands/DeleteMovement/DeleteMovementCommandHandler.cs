using Paytrack.Domain.Errors;
using Paytrack.Domain.Repositories;

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
            return MovementErrors.NotFound;

        await movementRepository.RemoveAsync(movement, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
