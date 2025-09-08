using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Application.UnitTests.UseCases.Users.Contracts;

public class AuthResponseTests
{
    [Fact]
    public void FromDomain_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var expectedToken = "jwt-token-123";

        // Act
        var response = AuthResponse.FromDomain(user, expectedToken);

        // Assert
        response.AccessToken.ShouldBe(expectedToken);
        response.User.ShouldSatisfyAllConditions(
            m => m.Id.ShouldBe(user.Id),
            m => m.Email.ShouldBe(user.Email),
            m => m.Currency.ShouldBe(user.Preferences.Currency),
            m => m.TimeZone.ShouldBe(user.Preferences.TimeZone));
    }
}
