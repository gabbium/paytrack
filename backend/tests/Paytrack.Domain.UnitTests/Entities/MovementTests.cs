using Paytrack.Domain.Enums;
using Paytrack.Domain.Resources;

namespace Paytrack.Domain.UnitTests.Entities;

public class MovementTests
{
    [Fact]
    public void Ctor_WhenValidArguments_ThenCreatesMovement()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var kind = MovementKind.Income;
        var amount = 123.45m;
        var description = "Salary";
        var occurredOn = DateTimeOffset.UtcNow;

        // Act
        var movement = new MovementBuilder()
            .WithUserId(userId)
            .WithKind(kind)
            .WithAmount(amount)
            .WithDescription(description)
            .WithOccurredOn(occurredOn)
            .Build();

        // Assert
        movement.ShouldSatisfyAllConditions(
            m => m.UserId.ShouldBe(userId),
            m => m.Kind.ShouldBe(kind),
            m => m.Amount.ShouldBe(amount),
            m => m.Description.ShouldBe(description),
            m => m.OccurredOn.ShouldBe(occurredOn));
    }

    [Fact]
    public void Ctor_WhenUserIdIsEmpty_ThenThrowsDomainException()
    {
        // Act & Assert
        var ex = Should.Throw<DomainException>(() =>
            new MovementBuilder().WithUserId(Guid.Empty).Build());

        ex.Error.Type.ShouldBe(ErrorType.Validation);
        ex.Error.Description.ShouldBe(Resource.Movement_UserId_NotEmpty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Ctor_WhenAmountIsZeroOrNegative_ThenThrowsDomainException(decimal invalidAmount)
    {
        // Act & Assert
        var ex = Should.Throw<DomainException>(() =>
            new MovementBuilder().WithAmount(invalidAmount).Build());

        ex.Error.Type.ShouldBe(ErrorType.Validation);
        ex.Error.Description.ShouldBe(string.Format(Resource.Movement_Amount_GreaterThan, 0));
    }

    [Fact]
    public void ChangeKind_UpdatesKind()
    {
        // Arrange
        var movement = new MovementBuilder().WithKind(MovementKind.Income).Build();

        // Act
        movement.ChangeKind(MovementKind.Expense);

        // Assert
        movement.Kind.ShouldBe(MovementKind.Expense);
    }

    [Fact]
    public void ChangeAmount_WhenValid_ThenUpdatesAmount()
    {
        // Arrange
        var movement = new MovementBuilder().WithAmount(100m).Build();

        // Act
        movement.ChangeAmount(200m);

        // Assert
        movement.Amount.ShouldBe(200m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ChangeAmount_WhenZeroOrNegative_ThenThrowsDomainException(decimal invalidAmount)
    {
        // Arrange
        var movement = new MovementBuilder().Build();

        // Act & Assert
        var ex = Should.Throw<DomainException>(() => movement.ChangeAmount(invalidAmount));

        ex.Error.Type.ShouldBe(ErrorType.Validation);
        ex.Error.Description.ShouldBe(string.Format(Resource.Movement_Amount_GreaterThan, 0));
    }

    [Fact]
    public void ChangeDescription_UpdatesDescription()
    {
        // Arrange
        var movement = new MovementBuilder().WithDescription("Test movement").Build();

        // Act
        movement.ChangeDescription("Test movement updated");

        // Assert
        movement.Description.ShouldBe("Test movement updated");
    }

    [Fact]
    public void ChangeOccurredOn_UpdatesDate()
    {
        // Arrange
        var movement = new MovementBuilder().WithOccurredOn(DateTimeOffset.UtcNow).Build();
        var newDate = DateTimeOffset.UtcNow.AddDays(-1);

        // Act
        movement.ChangeOccurredOn(newDate);

        // Assert
        movement.OccurredOn.ShouldBe(newDate);
    }
}
