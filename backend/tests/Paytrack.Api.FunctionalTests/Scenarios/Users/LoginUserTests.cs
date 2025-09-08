using Paytrack.Api.FunctionalTests.Assertions;
using Paytrack.Api.FunctionalTests.Steps;
using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Api.FunctionalTests.Scenarios.Users;

public class LoginUserTests(TestFixture fx) : TestBase(fx)
{
    private readonly AuthSteps _auth = new(fx);
    private readonly UserSteps _user = new(fx);

    [Fact]
    public async Task GivenAnonymousUser_WhenLoggingInWithValidCredentials_Then200WithBody()
    {
        await _auth.Given_AnonymousUser();
        var credentials = await _user.Given_RegisteredUserWithCredentials();
        var command = _user.Given_LoginCommand_WithEmailAndPassword(credentials.Email, credentials.Password);
        var response = await _user.When_AttemptToLoginAsync(command);
        await response.ShouldBeOkWithBody<AuthResponse>();
    }

    [Fact]
    public async Task GivenAnonymousUser_WhenLoggingInWithInvalidCredentials_Then401WithProblem()
    {
        await _auth.Given_AnonymousUser();
        var credentials = await _user.Given_RegisteredUserWithCredentials();
        var command = _user.Given_LoginCommand_WithEmailAndPassword(credentials.Email, "TotallyWrong#123");
        var response = await _user.When_AttemptToLoginAsync(command);
        await response.ShouldBeUnauthorizedWithProblem();
    }

    [Fact]
    public async Task GivenAnonymousUser_WhenLoggingInWithInvalidData_Then400WithValidation()
    {
        await _auth.Given_AnonymousUser();
        var command = _user.Given_InvalidLoginCommand_InvalidEmail();
        var response = await _user.When_AttemptToLoginAsync(command);
        await response.ShouldBeBadRequestWithValidation();
    }
}
