using Paytrack.Application.Common.Interfaces;
using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Domain.Entities;
using Paytrack.Domain.Repositories;

namespace Paytrack.Application.UseCases.Movements.Commands.CreateMovement;

internal sealed class CreateMovementCommandHandler(
    ICurrentUser currentUser,
    IMovementRepository movementRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMovementCommand, MovementResponse>
{
    public async Task<Result<MovementResponse>> HandleAsync(
        CreateMovementCommand command,
        CancellationToken cancellationToken = default)
    {
        var movement = new Movement(
            currentUser.UserId,
            command.Kind,
            command.Amount,
            command.Description,
            command.OccurredOn);

        await movementRepository.AddAsync(movement, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MovementResponse.FromDomain(movement);
    }
}