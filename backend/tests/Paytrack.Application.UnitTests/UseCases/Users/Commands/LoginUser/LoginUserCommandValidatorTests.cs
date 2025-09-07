using Paytrack.Application.UseCases.Users.Commands.LoginUser;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UnitTests.UseCases.Users.Commands.LoginUser;

public class LoginUserCommandValidatorTests
{
    private readonly LoginUserCommandValidator _validator = new();

    private static LoginUserCommand CreateValidCommand() =>
        new("user@example.com", "strongPassword123");

    [Fact]
    public void Validate_WhenCommandIsValid_ThenHasNoValidationErrors()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenEmailIsEmpty_ThenHasValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Email = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage(Resource.User_Email_NotEmpty);
    }

    [Fact]
    public void Validate_WhenEmailIsInvalid_ThenHasValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Email = "invalid-email" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage(Resource.User_Email_EmailAddress);
    }

    [Fact]
    public void Validate_WhenEmailExceedsMaxLength_ThenHasValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Email = new string('a', 257) + "@test.com" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage(string.Format(Resource.User_Email_MaxLength, 256));
    }

    [Fact]
    public void Validate_WhenPasswordIsEmpty_ThenHasValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Password = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage(Resource.User_Password_NotEmpty);
    }

    [Fact]
    public void Validate_WhenPasswordIsTooShort_ThenHasValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Password = "12345" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage(string.Format(Resource.User_Password_MinLength, 6));
    }

    [Fact]
    public void Validate_WhenPasswordExceedsMaxLength_ThenHasValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Password = new string('x', 129) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage(string.Format(Resource.User_Password_MaxLength, 128));
    }
}
