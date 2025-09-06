using Paytrack.Application.Common.Interfaces;
using Paytrack.Application.UseCases.Users.Contracts;
using Paytrack.Domain.Errors;
using Paytrack.Domain.Repositories;

namespace Paytrack.Application.UseCases.Users.Commands.LoginUser;

internal sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : ICommandHandler<LoginUserCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> HandleAsync(
        LoginUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null)
            return UserErrors.InvalidCredentials;

        if (!passwordHasher.Verify(command.Password, user.PasswordHash))
            return UserErrors.InvalidCredentials;

        var accessToken = tokenService.CreateAccessToken(user);

        return AuthResponse.FromDomain(user, accessToken);
    }
}
