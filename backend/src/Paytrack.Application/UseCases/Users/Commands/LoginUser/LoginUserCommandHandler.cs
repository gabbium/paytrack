using Paytrack.Application.Common.Interfaces;
using Paytrack.Application.UseCases.Users.Contracts;
using Paytrack.Domain.Repositories;
using Paytrack.Domain.Resources;

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
            return Error.Unauthorized(Resource.User_Login_InvalidCredentials);

        if (!passwordHasher.Verify(command.Password, user.PasswordHash))
            return Error.Unauthorized(Resource.User_Login_InvalidCredentials);

        var accessToken = tokenService.CreateAccessToken(user);

        return AuthResponse.FromDomain(user, accessToken);
    }
}
