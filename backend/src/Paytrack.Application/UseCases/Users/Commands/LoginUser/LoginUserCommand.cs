using Paytrack.Application.UseCases.Users.Contracts;

namespace Paytrack.Application.UseCases.Users.Commands.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password)
    : ICommand<AuthResponse>;
