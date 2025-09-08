using Paytrack.Domain.Events;

namespace Paytrack.Domain.UnitTests.Events;

public class MovementDeletedEventTests
{
    [Fact]
    public void FromDomain_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var movement = new MovementBuilder().Build();

        // Act
        var @event = MovementDeletedEvent.FromDomain(movement);

        // Assert
        @event.ShouldSatisfyAllConditions(
            e => e.Id.ShouldBe(movement.Id),
            e => e.UserId.ShouldBe(movement.UserId));
    }
}
