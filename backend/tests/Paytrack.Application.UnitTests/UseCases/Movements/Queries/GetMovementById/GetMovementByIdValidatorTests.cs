using Paytrack.Application.UseCases.Movements.Queries.GetMovementById;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UnitTests.UseCases.Movements.Queries.GetMovementById;

public class GetMovementByIdValidatorTests
{
    private readonly GetMovementByIdValidator _validator = new();

    private static GetMovementByIdQuery CreateValidQuery() =>
        new(Guid.NewGuid());

    [Fact]
    public void Validate_WhenQueryIsValid_ThenHasNoValidationErrors()
    {
        // Arrange
        var query = CreateValidQuery();

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenIdIsEmpty_ThenHasValidationError()
    {
        // Arrange
        var query = new GetMovementByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id)
            .WithErrorMessage(Resource.Movement_Id_NotEmpty);
    }
}
