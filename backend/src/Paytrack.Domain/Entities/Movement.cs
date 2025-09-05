using Paytrack.Domain.Enums;
using Paytrack.Domain.Exceptions;

namespace Paytrack.Domain.Entities;

public sealed class Movement : BaseAuditableEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public MovementKind Kind { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset OccurredOn { get; private set; }

    public Movement(Guid userId, MovementKind kind, decimal amount, DateTimeOffset occurredOn, string? description = null)
    {
        if (userId == Guid.Empty)
            throw new DomainRuleViolation("UserId is required", "The UserId cannot be empty.");

        if (amount <= 0)
            throw new DomainRuleViolation("Invalid amount", "The amount must be greater than zero.");

        UserId = userId;
        Kind = kind;
        Amount = amount;
        OccurredOn = occurredOn;
        Description = description;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void ChangeAmount(decimal newAmount)
    {
        if (newAmount <= 0)
            throw new DomainRuleViolation("Invalid amount", "The amount must be greater than zero.");

        Amount = newAmount;
    }
}
