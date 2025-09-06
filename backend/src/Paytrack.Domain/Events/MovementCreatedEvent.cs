using Paytrack.Domain.Entities;
using Paytrack.Domain.Enums;

namespace Paytrack.Domain.Events;

public sealed class MovementCreatedEvent(
    Guid id,
    Guid userId,
    MovementKind kind,
    decimal amount,
    string? description,
    DateTimeOffset occurredOn) 
    : DomainEventBase
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public MovementKind Kind { get; } = kind;
    public decimal Amount { get; } = amount;
    public string? Description { get; } = description;
    public DateTimeOffset OccurredOn { get; } = occurredOn;

    public static MovementCreatedEvent FromDomain(Movement movement) =>
        new(movement.Id,
            movement.UserId,
            movement.Kind,
            movement.Amount,
            movement.Description,
            movement.OccurredOn);
}
