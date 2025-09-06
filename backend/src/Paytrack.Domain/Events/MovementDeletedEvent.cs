using Paytrack.Domain.Entities;

namespace Paytrack.Domain.Events;

public sealed class MovementDeletedEvent(
    Guid id,
    Guid userId)
    : DomainEventBase
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;

    public static MovementDeletedEvent FromDomain(Movement movement) =>
        new(movement.Id,
            movement.UserId);
}
