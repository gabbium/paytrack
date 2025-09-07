using Paytrack.Application.UseCases.Movements.Queries.GetMovementById;

namespace Paytrack.Application.UnitTests.TestData;

public sealed class GetMovementByIdQueryBuilder
{
    private Guid _id = Guid.NewGuid();

    public GetMovementByIdQueryBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public GetMovementByIdQuery Build() => new(_id);
}
