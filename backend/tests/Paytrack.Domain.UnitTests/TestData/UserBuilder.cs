using Paytrack.Domain.Entities;

namespace Paytrack.Domain.UnitTests.TestData;

public sealed class UserBuilder
{
    private string _email = "user@example.com";
    private string _passwordHash = "hashed-password";

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public User Build() => new(_email, _passwordHash);
}
