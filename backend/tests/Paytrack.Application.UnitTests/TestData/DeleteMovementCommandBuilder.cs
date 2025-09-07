using Paytrack.Application.UseCases.Movements.Commands.DeleteMovement;

namespace Paytrack.Application.UnitTests.TestData;

public sealed class DeleteMovementCommandBuilder
{
    private Guid _id = Guid.NewGuid();

    public DeleteMovementCommandBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public DeleteMovementCommand Build() => new(_id);
}
