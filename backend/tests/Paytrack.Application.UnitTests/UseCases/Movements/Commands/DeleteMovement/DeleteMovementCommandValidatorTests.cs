using Paytrack.Application.UseCases.Movements.Commands.DeleteMovement;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UnitTests.UseCases.Movements.Commands.DeleteMovement;

public class DeleteMovementCommandValidatorTests
{
    private readonly DeleteMovementCommandValidator _validator = new();

    private static DeleteMovementCommand CreateValidCommand() =>
        new(Guid.NewGuid());

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
        var command = new DeleteMovementCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage(Resource.Movement_Id_NotEmpty);
    }
}
