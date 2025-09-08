using Paytrack.Api.FunctionalTests.Assertions;
using Paytrack.Api.FunctionalTests.Steps;
using Paytrack.Application.UseCases.Movements.Contracts;

namespace Paytrack.Api.FunctionalTests.Scenarios.Movements;

public class ListMovementsTests(TestFixture fx) : TestBase(fx)
{
    private readonly AuthSteps _auth = new(fx);
    private readonly MovementSteps _movement = new(fx);

    [Fact]
    public async Task GivenLoggedInUser_WhenListing_Then200WithBody()
    {
        await _auth.Given_LoggedInUser();
        await _movement.Given_ExistingMovement();
        await _movement.Given_ExistingMovement();
        var query = _movement.Given_ValidListQuery();
        var response = await _movement.When_AttemptToList(query);
        var list = await response.ShouldBeOkWithBody<PaginatedList<MovementResponse>>();
        list.Items.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GivenLoggedInUser_WhenListing_Then400WithValidation()
    {
        await _auth.Given_LoggedInUser();
        var query = _movement.Given_InvalidValidListQuery_PageNumberNegative();
        var response = await _movement.When_AttemptToList(query);
        await response.ShouldBeBadRequestWithValidation();
    }

    [Fact]
    public async Task GivenAnonymousUser_WhenListing_Then401WithBearerChallenge()
    {
        await _auth.Given_AnonymousUser();
        var query = _movement.Given_ValidListQuery();
        var response = await _movement.When_AttemptToList(query);
        response.ShouldBeUnauthorizedWithBearerChallenge();
    }
}
