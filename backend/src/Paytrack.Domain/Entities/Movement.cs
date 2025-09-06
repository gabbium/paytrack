using Paytrack.Domain.Enums;
using Paytrack.Domain.Resources;

namespace Paytrack.Domain.Entities;

public sealed class Movement : BaseAuditableEntity
{
    public Guid UserId { get; private set; }
    public MovementKind Kind { get; private set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset OccurredOn { get; private set; }

    public Movement(
        Guid userId,
        MovementKind kind,
        decimal amount,
        string? description,
        DateTimeOffset occurredOn)
    {
        if (userId == Guid.Empty)
            throw new DomainException(Error.Validation(Resource.Movement_UserId_NotEmpty));

        if (amount <= 0)
            throw new DomainException(Error.Validation(string.Format(Resource.Movement_Amount_GreaterThan, 0)));

        UserId = userId;
        Kind = kind;
        Amount = amount;
        Description = description;
        OccurredOn = occurredOn;
    }

    public void ChangeKind(MovementKind newKind)
    {
        Kind = newKind;
    }

    public void ChangeAmount(decimal newAmount)
    {
        if (newAmount <= 0)
            throw new DomainException(Error.Validation(string.Format(Resource.Movement_Amount_GreaterThan, 0)));

        Amount = newAmount;
    }

    public void ChangeDescription(string? newDescription)
    {
        Description = newDescription;
    }

    public void ChangeOccurredOn(DateTimeOffset newOccurredOn)
    {
        OccurredOn = newOccurredOn;
    }
}
