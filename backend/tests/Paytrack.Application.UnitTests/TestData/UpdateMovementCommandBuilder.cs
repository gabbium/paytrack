using Paytrack.Application.UseCases.Movements.Commands.UpdateMovement;
using Paytrack.Domain.Enums;

namespace Paytrack.Application.UnitTests.TestData;

public sealed class UpdateMovementCommandBuilder
{
    private Guid _id = Guid.NewGuid();
    private MovementKind _kind = MovementKind.Income;
    private decimal _amount = 100m;
    private string? _description = "Updated movement";
    private DateTimeOffset _occurredOn = DateTimeOffset.UtcNow;

    public UpdateMovementCommandBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UpdateMovementCommandBuilder WithKind(MovementKind kind)
    {
        _kind = kind;
        return this;
    }

    public UpdateMovementCommandBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public UpdateMovementCommandBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public UpdateMovementCommandBuilder WithOccurredOn(DateTimeOffset occurredOn)
    {
        _occurredOn = occurredOn;
        return this;
    }

    public UpdateMovementCommand Build() =>
        new(_id, _kind, _amount, _description, _occurredOn);
}
