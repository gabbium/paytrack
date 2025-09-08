using Paytrack.Domain.Resources;
using Paytrack.Domain.ValueObjects;

namespace Paytrack.Domain.Entities;

public sealed class User : BaseAuditableEntity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserPreferences Preferences { get; private set; }

    public User(string email, string passwordHash)
    {
        if (string.IsNullOrEmpty(email))
            throw new DomainException(Error.Validation(Resource.User_Email_NotEmpty));

        if (string.IsNullOrEmpty(passwordHash))
            throw new DomainException(Error.Validation(Resource.User_Password_NotEmpty));

        Email = email;
        PasswordHash = passwordHash;
        Preferences = UserPreferences.Default();
    }

    public void UpdatePreferences(string currency, string timeZone)
    {
        if (string.IsNullOrEmpty(currency))
            throw new DomainException(Error.Validation(Resource.User_Currency_NotEmpty));

        if (string.IsNullOrEmpty(timeZone))
            throw new DomainException(Error.Validation(Resource.User_TimeZone_NotEmpty));

        Preferences = new UserPreferences(currency, timeZone);
    }
}
