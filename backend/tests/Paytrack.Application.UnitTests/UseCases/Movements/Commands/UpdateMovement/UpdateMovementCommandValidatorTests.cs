using Paytrack.Application.UseCases.Movements.Commands.UpdateMovement;
using Paytrack.Domain.Enums;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UnitTests.UseCases.Movements.Commands.UpdateMovement;

public class UpdateMovementCommandValidatorTests
{
    private readonly UpdateMovementCommandValidator _validator = new();

    private static UpdateMovementCommand CreateValidCommand() =>
        new(
            Guid.NewGuid(),
            MovementKind.Income,
            123.45m,
            "Salary",
            DateTimeOffset.UtcNow
        );

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
    public void Validate_WhenIdIsEmpty_ThenHasValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage(Resource.Movement_Id_NotEmpty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenAmountIsZeroOrNegative_ThenHasValidationError(decimal invalidAmount)
    {
        // Arrange
        var command = CreateValidCommand() with { Amount = invalidAmount };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Amount)
            .WithErrorMessage(string.Format(Resource.Movement_Amount_GreaterThan, 0));
    }

    [Fact]
    public void Validate_WhenAmountHasTooManyDecimals_ThenHasValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Amount = 1.999m };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Amount)
            .WithErrorMessage(string.Format(Resource.Movement_Amount_PrecisionScale, 18, 2));
    }

    [Fact]
    public void Validate_WhenDescriptionExceedsMaxLength_ThenHasValidationError()
    {
        // Arrange
        var command = CreateValidCommand() with { Description = new string('a', 129) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(string.Format(Resource.Movement_Description_MaxLength, 128));
    }
}
