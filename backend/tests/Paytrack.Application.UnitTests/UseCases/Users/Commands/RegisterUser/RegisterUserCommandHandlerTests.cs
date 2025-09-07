using Paytrack.Application.Common.Interfaces;
using Paytrack.Application.UseCases.Users.Commands.RegisterUser;
using Paytrack.Domain.Entities;
using Paytrack.Domain.Repositories;
using Paytrack.Domain.Resources;
using Paytrack.Domain.ValueObjects;

namespace Paytrack.Application.UnitTests.UseCases.Users.Commands.RegisterUser;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new RegisterUserCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _unitOfWorkMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenEmailDoesNotExist_ThenCreatesUserAndReturnsSuccess()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "user@example.com",
            "plain-password");

        var hashedPassword = "hashed-password";

        var expectedToken = "jwt-token-123";

        _userRepositoryMock
            .Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasherMock
            .Setup(h => h.Hash(command.Password))
            .Returns(hashedPassword);

        _tokenServiceMock
            .Setup(t => t.CreateAccessToken(It.IsAny<User>()))
            .Returns(expectedToken);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();

        _userRepositoryMock.Verify(r =>
            r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()), Times.Once);

        _passwordHasherMock.Verify(u =>
            u.Hash(command.Password), Times.Once);

        _userRepositoryMock.Verify(r =>
            r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        _tokenServiceMock.Verify(t =>
            t.CreateAccessToken(It.IsAny<User>()), Times.Once);

        _userRepositoryMock.VerifyNoOtherCalls();

        _passwordHasherMock.VerifyNoOtherCalls();

        _unitOfWorkMock.VerifyNoOtherCalls();

        _tokenServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HandleAsync_WhenEmailAlreadyExists_ThenReturnsConflictError()
    {
        // Arrange
        var command = new RegisterUserCommand(
            "user@example.com",
            "plain-password");

        _userRepositoryMock
            .Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Type.ShouldBe(ErrorType.Conflict);
        result.Error.Description.ShouldBe(Resource.User_Email_AlreadyInUse);

        _userRepositoryMock.Verify(r =>
            r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>()), Times.Once);

        _userRepositoryMock.VerifyNoOtherCalls();

        _passwordHasherMock.VerifyNoOtherCalls();

        _tokenServiceMock.VerifyNoOtherCalls();
    }
}
