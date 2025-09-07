using Paytrack.Application.UseCases.Users.Contracts;
using Paytrack.Domain.Entities;

namespace Paytrack.Application.UnitTests.UseCases.Users.Contracts;

public class UserResponseTests
{
    [Fact]
    public void FromDomain_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var user = new User("user@example.com", "hashed-pass");

        // Act
        var response = UserResponse.FromDomain(user);

        // Assert
        response.ShouldSatisfyAllConditions(
            m => m.Id.ShouldBe(user.Id),
            m => m.Email.ShouldBe(user.Email),
            m => m.Currency.ShouldBe(user.Preferences.Currency),
            m => m.TimeZone.ShouldBe(user.Preferences.TimeZone));
    }
}
