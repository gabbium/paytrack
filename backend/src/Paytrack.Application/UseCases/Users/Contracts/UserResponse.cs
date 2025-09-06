namespace Paytrack.Application.UseCases.Users.Contracts;

public sealed record UserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
    public string Currency { get; init; } = null!;
    public string TimeZone { get; init; } = null!;
}