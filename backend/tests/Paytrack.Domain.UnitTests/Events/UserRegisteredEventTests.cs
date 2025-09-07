using Paytrack.Domain.Entities;
using Paytrack.Domain.Events;

namespace Paytrack.Domain.UnitTests.Events;

public class UserRegisteredEventTests
{
    [Fact]
    public void FromDomain_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var user = new User("user@example.com", "strongPassword123");

        // Act
        var @event = UserRegisteredEvent.FromDomain(user);

        // Assert
        @event.ShouldSatisfyAllConditions(
            e => e.Id.ShouldBe(user.Id),
            e => e.Email.ShouldBe(user.Email),
            e => e.Currency.ShouldBe(user.Preferences.Currency),
            e => e.TimeZone.ShouldBe(user.Preferences.TimeZone));
    }
}
