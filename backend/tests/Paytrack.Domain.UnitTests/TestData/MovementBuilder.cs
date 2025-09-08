using Paytrack.Domain.Entities;
using Paytrack.Domain.Enums;

namespace Paytrack.Domain.UnitTests.TestData;

public sealed class MovementBuilder
{
    private Guid _userId = Guid.NewGuid();
    private MovementKind _kind = MovementKind.Income;
    private decimal _amount = 100m;
    private string? _description = "Test movement";
    private DateTimeOffset _occurredOn = DateTimeOffset.UtcNow;

    public MovementBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public MovementBuilder WithKind(MovementKind kind)
    {
        _kind = kind;
        return this;
    }

    public MovementBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public MovementBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public MovementBuilder WithOccurredOn(DateTimeOffset occurredOn)
    {
        _occurredOn = occurredOn;
        return this;
    }

    public Movement Build() =>
        new(_userId, _kind, _amount, _description, _occurredOn);
}
