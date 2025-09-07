using Paytrack.Domain.Entities;
using Paytrack.Domain.Resources;
using Paytrack.Domain.ValueObjects;

namespace Paytrack.Domain.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void Ctor_WhenValidArguments_ThenCreatesUser()
    {
        // Arrange
        var email = "user@example.com";
        var passwordHash = "strongPassword123";
        var defaultPreferences = UserPreferences.Default();

        // Act
        var user = new User(email, passwordHash);

        // Assert
        user.ShouldSatisfyAllConditions(
            u => u.Email.ShouldBe(email),
            u => u.PasswordHash.ShouldBe(passwordHash),
            u => u.Preferences.ShouldBe(defaultPreferences));
    }

    [Fact]
    public void Ctor_WhenEmailIsEmpty_ThenThrowsDomainException()
    {
        // Act & Assert
        var ex = Should.Throw<DomainException>(() => new User(string.Empty, "strongPassword123"));

        ex.Error.Type.ShouldBe(ErrorType.Validation);
        ex.Error.Description.ShouldBe(Resource.User_Email_NotEmpty);
    }

    [Fact]
    public void Ctor_WhenPasswordHashIsEmpty_ThenThrowsDomainException()
    {
        // Act & Assert
        var ex = Should.Throw<DomainException>(() => new User("user@example.com", string.Empty));

        ex.Error.Type.ShouldBe(ErrorType.Validation);
        ex.Error.Description.ShouldBe(Resource.User_Password_NotEmpty);
    }

    [Fact]
    public void UpdatePreferences_UpdatesUserPreferences()
    {
        // Arrange
        var user = new User("user@example.com", "strongPassword123");
        var newCurrency = "EUR";
        var newTimeZone = "Europe/Berlin";

        // Act
        user.UpdatePreferences(newCurrency, newTimeZone);

        // Assert
        user.ShouldSatisfyAllConditions(
            u => u.Preferences.Currency.ShouldBe(newCurrency),
            u => u.Preferences.TimeZone.ShouldBe(newTimeZone));
    }

    [Fact]
    public void UpdatePreferences_WhenCurrencyIsEmpty_ThenThrowsDomainException()
    {
        // Arrange
        var user = new User("user@example.com", "strongPassword123");

        // Act & Assert
        var ex = Should.Throw<DomainException>(() => user.UpdatePreferences(string.Empty, "Europe/Berlin"));

        ex.Error.Type.ShouldBe(ErrorType.Validation);
        ex.Error.Description.ShouldBe(Resource.User_Currency_NotEmpty);
    }

    [Fact]
    public void UpdatePreferences_WhenTimeZoneIsEmpty_ThenThrowsDomainException()
    {
        // Arrange
        var user = new User("user@example.com", "strongPassword123");

        // Act & Assert
        var ex = Should.Throw<DomainException>(() => user.UpdatePreferences("EUR", string.Empty));

        ex.Error.Type.ShouldBe(ErrorType.Validation);
        ex.Error.Description.ShouldBe(Resource.User_TimeZone_NotEmpty);
    }
}
