using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Application.UnitTests.UseCases.Users.Contracts;

public class UserResponseTests
{
    [Fact]
    public void FromDomain_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var user = new UserBuilder().Build();

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
