using Paytrack.Domain.Errors;
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
            throw new DomainException(UserErrors.EmptyEmail);

        if (string.IsNullOrEmpty(passwordHash))
            throw new DomainException(UserErrors.EmptyPasswordHash);

        Email = email;
        PasswordHash = passwordHash;
        Preferences = UserPreferences.Default();
    }

    public void UpdatePreferences(string currency, string timeZone)
    {
        if (string.IsNullOrEmpty(currency))
            throw new DomainException(UserErrors.EmptyCurrency);

        if (string.IsNullOrEmpty(timeZone))
            throw new DomainException(UserErrors.EmptyTimeZone);

        Preferences = new UserPreferences(currency, timeZone);
    }
}
