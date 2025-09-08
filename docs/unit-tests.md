# Unit Testing Guidelines (Updated)

This document reflects the **exact testing style** used in Paytrack’s unit tests.

---

## 📌 Naming Conventions

- **Test classes**: `*Tests` (e.g., `MovementTests`, `CreateMovementCommandHandlerTests`).
- **Methods**: `MemberUnderTest_WhenCondition_ThenExpectedOutcome`
  - Examples:
    - `Ctor_WhenValidArguments_ThenCreatesMovement`
    - `Ctor_WhenUserIdIsEmpty_ThenThrowsDomainException`
    - `ChangeAmount_WhenZeroOrNegative_ThenThrowsDomainException`
    - `HandleAsync_WhenValid_ThenPersistsAndReturnsSuccess`

---

## 🧱 Test Structure

We follow **AAA** (Arrange / Act / Assert) explicitly in each test.

```csharp
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
```

---

## 🧰 Builders

- Builders are used for both **Domain** and **Application** layers to simplify object creation.
- Typical locations:
  - `Paytrack.Domain.UnitTests.TestData.MovementBuilder`
  - `Paytrack.Application.UnitTests.TestData.CreateMovementCommandBuilder`

```csharp
public sealed class MovementBuilder
{
    private Guid _userId = Guid.NewGuid();
    private MovementKind _kind = MovementKind.Income;
    private decimal _amount = 100m;
    private string? _description = "Test movement";
    private DateTimeOffset _occurredOn = DateTimeOffset.UtcNow;

    public MovementBuilder WithUserId(Guid userId) { _userId = userId; return this; }
    public MovementBuilder WithKind(MovementKind kind) { _kind = kind; return this; }
    public MovementBuilder WithAmount(decimal amount) { _amount = amount; return this; }
    public MovementBuilder WithDescription(string? description) { _description = description; return this; }
    public MovementBuilder WithOccurredOn(DateTimeOffset occurredOn) { _occurredOn = occurredOn; return this; }

    public Movement Build() => new(_userId, _kind, _amount, _description, _occurredOn);
}
```

---

## ✅ Assertions (Shouldly)

- Prefer **Shouldly** for readable assertions.
- Use `ShouldSatisfyAllConditions` to group property assertions.
- Use `Should.Throw<T>` for exceptions.

```csharp
[Theory]
[InlineData(0)]
[InlineData(-1)]
public void Ctor_WhenAmountIsZeroOrNegative_ThenThrowsDomainException(decimal invalidAmount)
{
    var ex = Should.Throw<DomainException>(() =>
        new MovementBuilder().WithAmount(invalidAmount).Build());

    ex.Error.Type.ShouldBe(ErrorType.Validation);
    ex.Error.Description.ShouldBe(string.Format(Resource.Movement_Amount_GreaterThan, 0));
}
```

---

## 🧪 Moq Usage & Verifications

- Mock only **external dependencies**.
- Verify calls explicitly and finish with **`VerifyNoOtherCalls()`** to catch leaks.

```csharp
[Fact]
public async Task HandleAsync_WhenValid_ThenPersistsAndReturnsSuccess()
{
    // Arrange
    var ctx = new Mock<IOperationContext>();
    var repo = new Mock<IMovementRepository>();
    var uow = new Mock<IUnitOfWork>();
    var handler = new CreateMovementCommandHandler(ctx.Object, repo.Object, uow.Object);
    var command = new CreateMovementCommandBuilder().Build();

    ctx.SetupGet(o => o.UserIdOrEmpty).Returns(Guid.NewGuid());

    // Act
    var result = await handler.HandleAsync(command);

    // Assert
    result.IsSuccess.ShouldBeTrue();
    result.Value.ShouldNotBeNull();

    ctx.Verify(o => o.UserIdOrEmpty, Times.Once());
    ctx.VerifyNoOtherCalls();

    repo.Verify(r => r.AddAsync(It.IsAny<Movement>(), It.IsAny<CancellationToken>()), Times.Once);
    repo.VerifyNoOtherCalls();

    uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    uow.VerifyNoOtherCalls();
}
```

---

## 🧠 Result Pattern

- Handlers return a **Result**-like type.
- Assert with `IsSuccess`, `Value`, or error content when applicable.

```csharp
result.IsSuccess.ShouldBeTrue();
result.Value.ShouldNotBeNull();
```

---

## 🧪 Theories

- Use `[Theory]` + `[InlineData]` for parameterized tests, especially for validation edge cases.

```csharp
[Theory]
[InlineData(0)]
[InlineData(-1)]
public void ChangeAmount_WhenZeroOrNegative_ThenThrowsDomainException(decimal invalidAmount)
{
    var movement = new MovementBuilder().Build();

    var ex = Should.Throw<DomainException>(() => movement.ChangeAmount(invalidAmount));

    ex.Error.Type.ShouldBe(ErrorType.Validation);
    ex.Error.Description.ShouldBe(string.Format(Resource.Movement_Amount_GreaterThan, 0));
}
```

---

## 🗂️ Organization & Namespaces

- Keep builders under `*.UnitTests.TestData` (separate for Domain/Application).
- Domain tests in `Paytrack.Domain.UnitTests.*` (e.g., `Entities`, `ValueObjects`).
- Application tests in `Paytrack.Application.UnitTests.*` (e.g., `UseCases`, `Services`).
