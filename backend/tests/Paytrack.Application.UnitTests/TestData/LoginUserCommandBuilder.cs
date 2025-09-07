using Paytrack.Application.UseCases.Users.Commands.LoginUser;

namespace Paytrack.Application.UnitTests.TestData;

public sealed class LoginUserCommandBuilder
{
    private string _email = "user@example.com";
    private string _password = "StrongPassword123!";

    public LoginUserCommandBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public LoginUserCommandBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public LoginUserCommand Build() => new(_email, _password);
}
