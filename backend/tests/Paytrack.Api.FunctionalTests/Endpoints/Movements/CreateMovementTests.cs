using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Api.FunctionalTests.Endpoints.Movements;

public class CreateMovementTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task UserCreatesMovement()
    {
        // Arrange
        await Fixture.RunAsDefaultUserAsync();

        var command = new CreateMovementCommandBuilder().Build();

        // Act
        var response = await Fixture.Client.PostAsJsonAsync("/api/movements", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<MovementResponse>(TestConstants.Json);
        body.ShouldNotBeNull();
        body.ShouldSatisfyAllConditions(
            m => m.Id.ShouldNotBe(Guid.Empty),
            m => m.Kind.ShouldBe(command.Kind),
            m => m.Amount.ShouldBe(command.Amount),
            m => m.Description.ShouldBe(command.Description),
            m => m.OccurredOn.ShouldBe(command.OccurredOn));
    }

    [Fact]
    public async Task AnonymousUserAttemptsToCreateMovement()
    {
        // Arrange
        var command = new CreateMovementCommandBuilder().Build();

        // Act
        var response = await Fixture.Client.PostAsJsonAsync("/api/movements", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
