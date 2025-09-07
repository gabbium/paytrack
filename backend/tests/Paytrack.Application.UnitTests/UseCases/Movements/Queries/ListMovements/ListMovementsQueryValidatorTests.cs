using Paytrack.Application.UseCases.Movements.Queries.ListMovements;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UnitTests.UseCases.Movements.Queries.ListMovements;

public class ListMovementsQueryValidatorTests
{
    private readonly ListMovementsQueryValidator _validator = new();

    private static ListMovementsQuery CreateValidQuery() =>
        new(1, 10);

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

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenPageNumberIsZeroOrNegative_ThenHasValidationError(int invalidPageNumber)
    {
        // Arrange
        var query = CreateValidQuery() with { PageNumber = invalidPageNumber };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PageNumber)
            .WithErrorMessage(string.Format(Resource.Movement_PageNumber_GreaterThan, 0));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenPageSizeIsZeroOrNegative_ThenHasValidationError(int invalidPageSize)
    {
        // Arrange
        var query = CreateValidQuery() with { PageSize = invalidPageSize };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PageSize)
            .WithErrorMessage(string.Format(Resource.Movement_PageSize_GreaterThan, 0));
    }

    [Fact]
    public void Validate_WhenPageSizeExceedsMax_ThenHasValidationError()
    {
        // Arrange
        var query = CreateValidQuery() with { PageSize = 101 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PageSize)
            .WithErrorMessage(string.Format(Resource.Movement_PageSize_Max, 100));
    }
}
