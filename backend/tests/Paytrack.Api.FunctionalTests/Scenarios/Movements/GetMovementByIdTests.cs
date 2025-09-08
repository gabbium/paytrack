using Paytrack.Api.FunctionalTests.Assertions;
using Paytrack.Api.FunctionalTests.Steps;
using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Api.FunctionalTests.Scenarios.Movements;

public class GetMovementByIdTests(TestFixture fx) : TestBase(fx)
{
    private readonly AuthSteps _auth = new(fx);
    private readonly MovementSteps _movement = new(fx);

    [Fact]
    public async Task GivenLoggedInUser_WhenGettingExisting_Then200WithBody()
    {
        await _auth.Given_LoggedInUser();
        var movement = await _movement.Given_ExistingMovement();
        var response = await _movement.When_AttemptToGetById(movement.Id);
        var body = await response.ShouldBeOkWithBody<MovementResponse>();
        body.Id.ShouldBe(movement.Id);
    }

    [Fact]
    public async Task GivenLoggedInUser_WhenGettingExistingOtherUsers_Then404WithProblem()
    {
        await _auth.Given_LoggedInUser();
        var otherUsersMovement = await _movement.Given_ExistingMovement();
        await _auth.Given_LoggedInUser();
        var response = await _movement.When_AttemptToGetById(otherUsersMovement.Id);
        await response.ShouldBeNotFoundWithProblem();
    }

    [Fact]
    public async Task GivenLoggedInUser_WhenGettingNonExistent_Then404WithProblem()
    {
        await _auth.Given_LoggedInUser();
        var nonExistentMovementId = Guid.NewGuid();
        var response = await _movement.When_AttemptToGetById(nonExistentMovementId);
        await response.ShouldBeNotFoundWithProblem();
    }

    [Fact]
    public async Task GivenLoggedInUser_WhenGettingWithInvalidId_Then400WithValidation()
    {
        await _auth.Given_LoggedInUser();
        var invalidMovementId = Guid.Empty;
        var response = await _movement.When_AttemptToGetById(invalidMovementId);
        await response.ShouldBeBadRequestWithValidation();
    }

    [Fact]
    public async Task GivenAnonymousUser_WhenGetting_Then401WithBearerChallenge()
    {
        await _auth.Given_AnonymousUser();
        var movementId = Guid.NewGuid();
        var response = await _movement.When_AttemptToGetById(movementId);
        response.ShouldBeUnauthorizedWithBearerChallenge();
    }
}
