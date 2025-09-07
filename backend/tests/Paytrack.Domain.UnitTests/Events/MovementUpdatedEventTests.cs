using Paytrack.Domain.Events;

namespace Paytrack.Domain.UnitTests.Events;

public class MovementUpdatedEventTests
{
    [Fact]
    public void FromDomain_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var movement = new MovementBuilder().Build();

        // Act
        var @event = MovementUpdatedEvent.FromDomain(movement);

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
