using Paytrack.Domain.Exceptions;
using Paytrack.Domain.ValueObjects;

namespace Paytrack.Domain.Entities;

public sealed class User : BaseAuditableEntity, IAggregateRoot
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserPreferences Preferences { get; private set; }

    public User(string email, string passwordHash)
    {
        if (string.IsNullOrEmpty(email))
            throw new DomainRuleViolation("Email is required", "The email cannot be empty or whitespace.");

        if (string.IsNullOrEmpty(passwordHash))
            throw new DomainRuleViolation("Password is required", "The password hash cannot be empty or whitespace.");

        Email = email;
        PasswordHash = passwordHash;
        Preferences = UserPreferences.Default();
    }

    public void UpdatePreferences(string currency, string timeZone)
    {
        if (string.IsNullOrEmpty(currency))
            throw new DomainRuleViolation("Currency is required", "The currency code cannot be empty or whitespace.");

        if (string.IsNullOrEmpty(timeZone))
            throw new DomainRuleViolation("Time zone is required", "The time zone cannot be empty or whitespace.");

        Preferences = new UserPreferences(currency, timeZone);
    }
}
