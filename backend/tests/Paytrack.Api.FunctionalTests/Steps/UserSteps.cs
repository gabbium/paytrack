using Paytrack.Application.UseCases.Users.Commands.LoginUser;
using Paytrack.Application.UseCases.Users.Commands.RegisterUser;
using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Api.FunctionalTests.Steps;

public sealed class UserSteps(TestFixture fx)
{
    public readonly record struct Credentials(string Email, string Password);

    public RegisterUserCommand Given_ValidRegisterCommand()
    {
        return new RegisterUserCommandBuilder().Build();
    }

    public RegisterUserCommand Given_InvalidRegisterCommand_InvalidEmail()
    {
        return new RegisterUserCommandBuilder()
                .WithEmail("invalid-email")
                .Build();
    }

    public RegisterUserCommand Given_RegisterCommand_WithEmail(string email)
    {
        return new RegisterUserCommandBuilder()
            .WithEmail(email)
            .Build();
    }

    public LoginUserCommand Given_ValidLoginCommand()
    {
        return new LoginUserCommandBuilder().Build();
    }

    public LoginUserCommand Given_InvalidLoginCommand_InvalidEmail()
    {
        return new LoginUserCommandBuilder()
                .WithEmail("invalid-email")
                .Build();
    }

    public LoginUserCommand Given_LoginCommand_WithEmailAndPassword(string email, string password)
        => new LoginUserCommandBuilder()
            .WithEmail(email)
            .WithPassword(password)
            .Build();

    public async Task<Credentials> Given_RegisteredUserWithCredentials()
    {
        var command = new RegisterUserCommandBuilder().Build();

        var response = await fx.Client.PostAsJsonAsync("/api/users/register", command);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<AuthResponse>(TestConstants.Json);
        body.ShouldNotBeNull();

        return new Credentials(command.Email, command.Password);
    }

    public async Task<HttpResponseMessage> When_AttemptToRegisterAsync(RegisterUserCommand command)
    {
        return await fx.Client.PostAsJsonAsync("/api/users/register", command);
    }

    public async Task<HttpResponseMessage> When_AttemptToLoginAsync(LoginUserCommand command)
    {
        return await fx.Client.PostAsJsonAsync("/api/users/login", command);
    }
}
