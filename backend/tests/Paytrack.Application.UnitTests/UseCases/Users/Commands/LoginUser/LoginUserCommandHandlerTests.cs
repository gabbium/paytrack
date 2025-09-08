using Paytrack.Application.Common.Interfaces;
using Paytrack.Application.UseCases.Users.Commands.LoginUser;
using Paytrack.Domain.Entities;
using Paytrack.Domain.Repositories;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UnitTests.UseCases.Users.Commands.LoginUser;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new LoginUserCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenCredentialsAreValid_ThenReturnsSuccess()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var command = new LoginUserCommandBuilder()
            .WithEmail(user.Email)
            .Build();
        var expectedToken = "jwt-token-123";

        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(h => h.Verify(command.Password, user.PasswordHash))
            .Returns(true);

        _tokenServiceMock
            .Setup(t => t.CreateAccessToken(user))
            .Returns(expectedToken);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();

        _userRepositoryMock.Verify(r =>
            r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()), Times.Once);

        _passwordHasherMock.Verify(u =>
            u.Verify(command.Password, user.PasswordHash), Times.Once);

        _tokenServiceMock.Verify(t =>
            t.CreateAccessToken(user), Times.Once);

        _userRepositoryMock.VerifyNoOtherCalls();

        _passwordHasherMock.VerifyNoOtherCalls();

        _tokenServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HandleAsync_WhenUserDoesNotExist_ThenReturnsUnauthorized()
    {
        // Arrange
        var command = new LoginUserCommandBuilder().Build();

        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Type.ShouldBe(ErrorType.Unauthorized);
        result.Error.Description.ShouldBe(Resource.User_Login_InvalidCredentials);

        _userRepositoryMock.Verify(r =>
            r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()), Times.Once);

        _userRepositoryMock.VerifyNoOtherCalls();

        _passwordHasherMock.VerifyNoOtherCalls();

        _tokenServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HandleAsync_WhenPasswordIsInvalid_ThenReturnsUnauthorized()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var command = new LoginUserCommandBuilder()
            .WithEmail(user.Email)
            .Build();

        _userRepositoryMock
            .Setup(r => r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(h => h.Verify(command.Password, user.PasswordHash))
            .Returns(false);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Type.ShouldBe(ErrorType.Unauthorized);
        result.Error.Description.ShouldBe(Resource.User_Login_InvalidCredentials);

        _userRepositoryMock.Verify(r =>
            r.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()), Times.Once);

        _passwordHasherMock.Verify(u =>
            u.Verify(command.Password, user.PasswordHash), Times.Once);

        _userRepositoryMock.VerifyNoOtherCalls();

        _passwordHasherMock.VerifyNoOtherCalls();

        _tokenServiceMock.VerifyNoOtherCalls();
    }
}
