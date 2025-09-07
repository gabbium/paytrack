using Paytrack.Domain.Entities;
using Paytrack.Domain.Enums;
using Paytrack.Domain.Events;

namespace Paytrack.Domain.UnitTests.Events;

public class MovementDeletedEventTests
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
        var @event = MovementDeletedEvent.FromDomain(movement);

        // Assert
        @event.ShouldSatisfyAllConditions(
            e => e.Id.ShouldBe(movement.Id),
            e => e.UserId.ShouldBe(movement.UserId));
    }
}
