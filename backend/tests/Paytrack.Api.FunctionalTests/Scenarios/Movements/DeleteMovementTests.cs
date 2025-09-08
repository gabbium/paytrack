using Paytrack.Api.FunctionalTests.Assertions;
using Paytrack.Api.FunctionalTests.Steps;

namespace Paytrack.Api.FunctionalTests.Scenarios.Movements;

public class DeleteMovementTests(TestFixture fx) : TestBase(fx)
{
    private readonly AuthSteps _auth = new(fx);
    private readonly MovementSteps _movement = new(fx);

    [Fact]
    public async Task GivenLoggedInUser_WhenDeletingExisting_Then204()
    {
        await _auth.Given_LoggedInUser();
        var movement = await _movement.Given_ExistingMovement();
        var response = await _movement.When_AttemptToDelete(movement.Id);
        response.ShouldBeNoContent();
    }

    [Fact]
    public async Task GivenLoggedInUser_WhenDeletingNonExistent_Then404WithProblem()
    {
        await _auth.Given_LoggedInUser();
        var nonExistentMovementId = Guid.NewGuid();
        var response = await _movement.When_AttemptToDelete(nonExistentMovementId);
        await response.ShouldBeNotFoundWithProblem();
    }

    [Fact]
    public async Task GivenLoggedInUser_WhenDeletingWithInvalidId_Then400WithValidation()
    {
        await _auth.Given_LoggedInUser();
        var invalidMovementId = Guid.Empty;
        var response = await _movement.When_AttemptToDelete(invalidMovementId);
        await response.ShouldBeBadRequestWithValidation();
    }

    [Fact]
    public async Task GivenAnonymousUser_WhenDeleting_Then401WithBearerChallenge()
    {
        await _auth.Given_AnonymousUser();
        var movementId = Guid.NewGuid();
        var response = await _movement.When_AttemptToDelete(movementId);
        response.ShouldBeUnauthorizedWithBearerChallenge();
    }
}
