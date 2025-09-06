using Paytrack.Application.Common.Interfaces;
using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Domain.Entities;
using Paytrack.Domain.Events;
using Paytrack.Domain.Repositories;

namespace Paytrack.Application.UseCases.Movements.Commands.CreateMovement;

internal sealed class CreateMovementCommandHandler(
    IOperationContext operationContext,
    IMovementRepository movementRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMovementCommand, MovementResponse>
{
    public async Task<Result<MovementResponse>> HandleAsync(
        CreateMovementCommand command,
        CancellationToken cancellationToken = default)
    {
        var movement = new Movement(
            operationContext.UserIdOrEmpty,
            command.Kind,
            command.Amount,
            command.Description,
            command.OccurredOn);

        movement.AddDomainEvent(MovementCreatedEvent.FromDomain(movement));

        await movementRepository.AddAsync(movement, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MovementResponse.FromDomain(movement);
    }
}