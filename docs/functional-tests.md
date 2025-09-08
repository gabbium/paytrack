# Functional Testing Guidelines (Updated)

This document reflects the **exact testing style** used in Paytrack’s functional tests.

---

## 📌 Naming Conventions

- **Test classes**: `*Tests` grouped by **Scenario** (e.g., `CreateMovementTests`, `DeleteMovementTests`).
- **Methods**: `Given_When_Then` style for BDD clarity.
  - Examples:
    - `GivenLoggedInUser_WhenCreatingWithValidData_Then201WithBodyAndLocation`
    - `GivenAnonymousUser_WhenDeleting_Then401WithBearerChallenge`

---

## 🧱 Structure

Functional tests are organized into 3 main parts:

1. **Steps** (`*.Steps`) – reusable actions and data builders.
2. **Assertions** (`*.Assertions`) – custom extension methods for HTTP responses.
3. **Scenarios** (`*.Scenarios`) – actual test cases, composed of steps + assertions.

---

## 🛠️ Assertions

Custom assertion extensions are defined under `Paytrack.Api.FunctionalTests.Assertions`.  
They provide fluent checks on `HttpResponseMessage` with deserialization.

Examples:

```csharp
await response.ShouldBeCreatedWithBodyAndLocation<MovementResponse>(
    body => $"/api/movements/{body.Id}");

await response.ShouldBeBadRequestWithValidation();

response.ShouldBeUnauthorizedWithBearerChallenge();
```

Available helpers:

- `ShouldBeCreatedWithBodyAndLocation<T>`
- `ShouldBeOkWithBody<T>`
- `ShouldBeNoContent()`
- `ShouldBeBadRequestWithValidation()`
- `ShouldBeBadRequestWithProblem()`
- `ShouldBeNotFoundWithProblem()`
- `ShouldBeConflictWithProblem()`
- `ShouldBeUnauthorizedWithBearerChallenge()`
- `ShouldBeUnauthorizedWithProblem()`

---

## 🛠️ Steps

Steps encapsulate reusable **Given/When** logic, e.g., authentication or movement setup.

Example (`AuthSteps`):

```csharp
public Task Given_LoggedInUser()
{
    var user = new UserBuilder().Build();
    using var scope = fx.Factory.Services.CreateScope();
    var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
    var accessToken = tokenService.CreateAccessToken(user);
    fx.Client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", accessToken);
    return Task.CompletedTask;
}

public Task Given_AnonymousUser()
{
    fx.Client.DefaultRequestHeaders.Authorization = null;
    return Task.CompletedTask;
}
```

Example (`MovementSteps`):

- `Given_ValidCreateCommand()`
- `Given_InvalidCreateCommand_TooLongDescription()`
- `Given_ExistingMovement()`
- `When_AttemptToCreate(...)`
- `When_AttemptToDelete(...)`
- etc.

---

## 🧪 Scenarios

Each scenario composes steps + assertions into a **complete BDD-style test**.

Example (`CreateMovementTests`):

```csharp
[Fact]
public async Task GivenLoggedInUser_WhenCreatingWithValidData_Then201WithBodyAndLocation()
{
    await _auth.Given_LoggedInUser();
    var command = _movement.Given_ValidCreateCommand();
    var response = await _movement.When_AttemptToCreate(command);
    await response.ShouldBeCreatedWithBodyAndLocation<MovementResponse>(
        body => $"/api/movements/{body.Id}");
}
```

Example (`DeleteMovementTests`):

```csharp
[Fact]
public async Task GivenLoggedInUser_WhenDeletingNonExistent_Then404WithProblem()
{
    await _auth.Given_LoggedInUser();
    var nonExistentMovementId = Guid.NewGuid();
    var response = await _movement.When_AttemptToDelete(nonExistentMovementId);
    await response.ShouldBeNotFoundWithProblem();
}
```

---

## ✅ Best Practices

- Always follow **Given/When/Then** naming and structure.
- Encapsulate data creation and actions in **Steps**, not directly in scenarios.
- Encapsulate HTTP assertions in **Assertions** extensions for readability.
- Reset DB state between scenarios (via **Testcontainers + Respawn**).
- Reuse builders from Application layer for commands/queries.

---

## 🗂️ Organization & Namespaces

- `Paytrack.Api.FunctionalTests.Assertions` → custom HTTP response assertions
- `Paytrack.Api.FunctionalTests.Steps` → reusable steps (auth, movements, etc.)
- `Paytrack.Api.FunctionalTests.Scenarios` → scenario-based test classes
