using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Application.UseCases.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string Password)
    : ICommand<AuthResponse>;
