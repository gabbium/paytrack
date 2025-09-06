using Paytrack.Domain.Entities;

namespace Paytrack.Application.UseCases.Users.Contracts;

public sealed record AuthResponse
{
    public UserResponse User { get; init; } = default!;
    public string AccessToken { get; init; } = null!;

    public static AuthResponse FromDomain(User user, string accessToken) =>
        new()
        {
            User = UserResponse.FromDomain(user),
            AccessToken = accessToken
        };
}
