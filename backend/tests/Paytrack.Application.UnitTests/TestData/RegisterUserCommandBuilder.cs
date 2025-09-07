using Paytrack.Application.UseCases.Users.Commands.RegisterUser;

namespace Paytrack.Application.UnitTests.TestData;

public sealed class RegisterUserCommandBuilder
{
    private string _email = "user@example.com";
    private string _password = "StrongPassword123!";

    public RegisterUserCommandBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public RegisterUserCommandBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public RegisterUserCommand Build() => new(_email, _password);
}
