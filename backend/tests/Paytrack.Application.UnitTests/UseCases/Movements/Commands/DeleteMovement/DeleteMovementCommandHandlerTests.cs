using Paytrack.Application.UseCases.Movements.Commands.DeleteMovement;
using Paytrack.Domain.Entities;
using Paytrack.Domain.Enums;
using Paytrack.Domain.Repositories;
using Paytrack.Domain.Resources;

namespace Paytrack.Application.UnitTests.UseCases.Movements.Commands.DeleteMovement;

public class DeleteMovementCommandHandlerTests
{
    private readonly Mock<IMovementRepository> _movementRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteMovementCommandHandler _handler;

    public DeleteMovementCommandHandlerTests()
    {
        _movementRepositoryMock = new Mock<IMovementRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteMovementCommandHandler(
            _movementRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenMovementExists_ThenRemovesAndReturnsSuccess()
    {
        // Arrange
        var command = new DeleteMovementCommand(Guid.NewGuid());

        var movement = new Movement(
            Guid.NewGuid(),
            MovementKind.Income,
            123.45m,
            "Salary",
            DateTimeOffset.UtcNow);

        _movementRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(movement);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _movementRepositoryMock.Verify(r =>
            r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);

        _movementRepositoryMock.Verify(r =>
            r.RemoveAsync(movement, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        _movementRepositoryMock.VerifyNoOtherCalls();

        _unitOfWorkMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HandleAsync_WhenMovementNotFound_ThenReturnsNotFoundError()
    {
        // Arrange
        var command = new DeleteMovementCommand(Guid.NewGuid());

        _movementRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Movement?)null);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Type.ShouldBe(ErrorType.NotFound);
        result.Error.Description.ShouldBe(Resource.Movement_NotFound);

        _movementRepositoryMock.Verify(r =>
            r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);

        _movementRepositoryMock.VerifyNoOtherCalls();

        _unitOfWorkMock.VerifyNoOtherCalls();
    }
}
