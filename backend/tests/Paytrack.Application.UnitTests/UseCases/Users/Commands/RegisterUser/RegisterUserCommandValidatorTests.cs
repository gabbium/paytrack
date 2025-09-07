using Paytrack.Application.UseCases.Users.Commands.RegisterUser;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UnitTests.UseCases.Users.Commands.RegisterUser;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenCommandIsValid_ThenHasNoValidationErrors()
    {
        // Arrange
        var command = new RegisterUserCommandBuilder().Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenEmailIsEmpty_ThenHasValidationError()
    {
        // Arrange
        var command = new RegisterUserCommandBuilder()
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
        var command = new RegisterUserCommandBuilder()
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
        var command = new RegisterUserCommandBuilder()
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
        var command = new RegisterUserCommandBuilder()
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
        var command = new RegisterUserCommandBuilder()
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
        var command = new RegisterUserCommandBuilder()
            .WithPassword(new string('x', 129))
            .Build();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage(string.Format(Resource.User_Password_MaxLength, 128));
    }
}
