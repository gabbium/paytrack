using Paytrack.Domain.Entities;

namespace Paytrack.Domain.Events;

public sealed class UserRegisteredEvent(
    Guid userId,
    string email,
    string currency,
    string timeZone)
    : DomainEventBase
{
    public Guid UserId { get; } = userId;
    public string Email { get; } = email;
    public string Currency { get; } = currency;
    public string TimeZone { get; } = timeZone;

    public static UserRegisteredEvent FromDomain(User user) =>
        new(user.Id,
            user.Email,
            user.Preferences.Currency,
            user.Preferences.TimeZone);
}
