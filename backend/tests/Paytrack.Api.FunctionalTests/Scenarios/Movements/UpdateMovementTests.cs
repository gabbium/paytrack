using Paytrack.Api.FunctionalTests.Assertions;
using Paytrack.Api.FunctionalTests.Steps;
using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Api.FunctionalTests.Scenarios.Movements;

public class UpdateMovementTests(TestFixture fx) : TestBase(fx)
{
    private readonly AuthSteps _auth = new(fx);
    private readonly MovementSteps _movement = new(fx);

    [Fact]
    public async Task GivenLoggedInUser_WhenUpdatingWithValidData_Then200WithBody()
    {
        await _auth.Given_LoggedInUser();
        var movement = await _movement.Given_ExistingMovement();
        var command = _movement.Given_ValidUpdateCommand(movement.Id);
        var response = await _movement.When_AttemptToUpdate(movement.Id, command);
        var body = await response.ShouldBeOkWithBody<MovementResponse>();
        body.Id.ShouldBe(movement.Id);
    }

    [Fact]
    public async Task GivenLoggedInUser_WhenUpdatingNonExistent_Then404WithProblem()
    {
        await _auth.Given_LoggedInUser();
        var nonExistentMovementId = Guid.NewGuid();
        var command = _movement.Given_ValidUpdateCommand(nonExistentMovementId);
        var response = await _movement.When_AttemptToUpdate(nonExistentMovementId, command);
        await response.ShouldBeNotFoundWithProblem();
    }

    [Fact]
    public async Task GivenLoggedInUser_WhenUpdatingWithInvalidData_Then400WithValidation()
    {
        await _auth.Given_LoggedInUser();
        var movementId = Guid.NewGuid();
        var command = _movement.Given_InvalidUpdateCommand_TooLongDescription(movementId);
        var response = await _movement.When_AttemptToUpdate(movementId, command);
        await response.ShouldBeBadRequestWithValidation();
    }

    [Fact]
    public async Task GivenAnonymousUser_WhenUpdating_Then401WithBearerChallenge()
    {
        await _auth.Given_AnonymousUser();
        var movementId = Guid.NewGuid();
        var command = _movement.Given_ValidUpdateCommand(movementId);
        var response = await _movement.When_AttemptToUpdate(movementId, command);
        response.ShouldBeUnauthorizedWithBearerChallenge();
    }
}
