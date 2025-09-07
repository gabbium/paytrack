using Paytrack.Application.UseCases.Users.Commands.LoginUser;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UnitTests.UseCases.Users.Commands.LoginUser;

public class LoginUserCommandValidatorTests
{
    private readonly LoginUserCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenCommandIsValid_ThenHasNoValidationErrors()
    {
        // Arrange
        var command = new LoginUserCommandBuilder().Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenEmailIsEmpty_ThenHasValidationError()
    {
        // Arrange
        var command = new LoginUserCommandBuilder()
            .WithEmail(string.Empty)
            .Build();

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
        var command = new LoginUserCommandBuilder()
            .WithEmail("invalid-email")
            .Build();

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
        var command = new LoginUserCommandBuilder()
            .WithEmail(new string('a', 257) + "@test.com")
            .Build();

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
        var command = new LoginUserCommandBuilder()
            .WithPassword(string.Empty)
            .Build();

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
        var command = new LoginUserCommandBuilder()
            .WithPassword("12345")
            .Build();

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
        var command = new LoginUserCommandBuilder()
            .WithPassword(new string('x', 129))
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage(string.Format(Resource.User_Password_MaxLength, 128));
    }
}
