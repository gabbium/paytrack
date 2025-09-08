using Paytrack.Api.FunctionalTests.Assertions;
using Paytrack.Api.FunctionalTests.Steps;
using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Api.FunctionalTests.Scenarios.Users;

public class RegisterUserTests(TestFixture fx) : TestBase(fx)
{
    private readonly AuthSteps _auth = new(fx);
    private readonly UserSteps _user = new(fx);

    [Fact]
    public async Task GivenAnonymousUser_WhenRegisteringWithValidData_Then200WithBody()
    {
        await _auth.Given_AnonymousUser();
        var command = _user.Given_ValidRegisterCommand();
        var response = await _user.When_AttemptToRegisterAsync(command);
        await response.ShouldBeOkWithBody<AuthResponse>();
    }

    [Fact]
    public async Task GivenAnonymousUser_WhenRegisteringWithExistingEmail_Then409WithProblem()
    {
        await _auth.Given_AnonymousUser();
        var credentials = await _user.Given_RegisteredUserWithCredentials();
        var command = _user.Given_RegisterCommand_WithEmail(credentials.Email);
        var response = await _user.When_AttemptToRegisterAsync(command);
        await response.ShouldBeConflictWithProblem();
    }

    [Fact]
    public async Task GivenAnonymousUser_WhenRegisteringInWithInvalidData_Then400WithValidation()
    {
        await _auth.Given_AnonymousUser();
        var command = _user.Given_InvalidRegisterCommand_InvalidEmail();
        var response = await _user.When_AttemptToRegisterAsync(command);
        await response.ShouldBeBadRequestWithValidation();
    }
}
