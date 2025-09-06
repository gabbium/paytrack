using Paytrack.Application.Common.Interfaces;
using Paytrack.Application.UseCases.Users.Contracts;
using Paytrack.Domain.Entities;
using Paytrack.Domain.Repositories;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UseCases.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    ITokenService tokenService)
    : ICommandHandler<RegisterUserCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Error.Conflict(Resource.User_Email_AlreadyInUse);

        var passwordHash = passwordHasher.Hash(command.Password);

        var user = new User(command.Email, passwordHash);

        await userRepository.AddAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = tokenService.CreateAccessToken(user);

        return AuthResponse.FromDomain(user, accessToken);
    }
}
