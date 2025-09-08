using Paytrack.Domain.Entities;

namespace Paytrack.Application.UseCases.Users.Contracts;

public sealed record UserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
    public string Currency { get; init; } = null!;
    public string TimeZone { get; init; } = null!;

    public static UserResponse FromDomain(User user) =>
        new()
        {
            Id = user.Id,
            Email = user.Email,
            Currency = user.Preferences.Currency,
            TimeZone = user.Preferences.TimeZone
        };
}
