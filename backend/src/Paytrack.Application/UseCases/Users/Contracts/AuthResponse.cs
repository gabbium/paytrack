namespace Paytrack.Application.UseCases.Users.Contracts;

public sealed record AuthResponse
{
    public UserResponse User { get; init; } = default!;
    public string AccessToken { get; init; } = null!;
}
