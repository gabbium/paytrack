using Paytrack.Domain.Entities;

namespace Paytrack.Domain.Events;

public sealed class UserRegisteredEvent(
    Guid id,
    string email,
    string currency,
    string timeZone)
    : DomainEventBase
{
    public Guid Id { get; } = id;
    public string Email { get; } = email;
    public string Currency { get; } = currency;
    public string TimeZone { get; } = timeZone;

    public static UserRegisteredEvent FromDomain(User user) =>
        new(user.Id,
            user.Email,
            user.Preferences.Currency,
            user.Preferences.TimeZone);
}
