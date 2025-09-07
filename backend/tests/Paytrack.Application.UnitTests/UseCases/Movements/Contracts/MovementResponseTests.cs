using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Domain.Entities;
using Paytrack.Domain.Enums;

namespace Paytrack.Application.UnitTests.UseCases.Movements.Contracts;

public class MovementResponseTests
{
    [Fact]
    public void FromDomain_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var movement = new Movement(
            Guid.NewGuid(),
            MovementKind.Income,
            123.45m,
            "Salary",
            DateTimeOffset.UtcNow);

        // Act
        var response = MovementResponse.FromDomain(movement);

        // Assert
        response.ShouldSatisfyAllConditions(
            m => m.Id.ShouldBe(movement.Id),
            m => m.Kind.ShouldBe(movement.Kind),
            m => m.Amount.ShouldBe(movement.Amount),
            m => m.Description.ShouldBe(movement.Description),
            m => m.OccurredOn.ShouldBe(movement.OccurredOn));
    }
}
