using Paytrack.Domain.Entities;
using Paytrack.Domain.Enums;
using Paytrack.Domain.Events;

namespace Paytrack.Domain.UnitTests.Events;

public class MovementCreatedEventTests
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
        var @event = MovementCreatedEvent.FromDomain(movement);

        // Assert
        @event.ShouldSatisfyAllConditions(
            e => e.Id.ShouldBe(movement.Id),
            e => e.UserId.ShouldBe(movement.UserId),
            e => e.Kind.ShouldBe(movement.Kind),
            e => e.Amount.ShouldBe(movement.Amount),
            e => e.Description.ShouldBe(movement.Description),
            e => e.OccurredOn.ShouldBe(movement.OccurredOn));
    }
}
