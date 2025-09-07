using Paytrack.Application.Common.Interfaces;
using Paytrack.Application.UseCases.Movements.Commands.CreateMovement;
using Paytrack.Domain.Entities;
using Paytrack.Domain.Enums;
using Paytrack.Domain.Repositories;

namespace Paytrack.Application.UnitTests.UseCases.Movements.Commands.CreateMovement;

public class CreateMovementCommandHandlerTests
{
    private readonly Mock<IOperationContext> _operationContextMock;
    private readonly Mock<IMovementRepository> _movementRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateMovementCommandHandler _handler;

    public CreateMovementCommandHandlerTests()
    {
        _operationContextMock = new Mock<IOperationContext>();
        _movementRepositoryMock = new Mock<IMovementRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateMovementCommandHandler(
            _operationContextMock.Object,
            _movementRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenValid_ThenPersistsAndReturnsSuccess()
    {
        // Arrange
        var command = new CreateMovementCommand(
            MovementKind.Income,
            123.45m,
            "Salary",
            DateTimeOffset.UtcNow);

        _operationContextMock
            .SetupGet(o => o.UserIdOrEmpty)
            .Returns(Guid.NewGuid());

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();

        _operationContextMock.Verify(o =>
            o.UserIdOrEmpty, Times.Once());
        _operationContextMock.VerifyNoOtherCalls();

        _movementRepositoryMock.Verify(r =>
            r.AddAsync(It.IsAny<Movement>(), It.IsAny<CancellationToken>()), Times.Once);
        _movementRepositoryMock.VerifyNoOtherCalls();

        _unitOfWorkMock.Verify(u =>
            u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.VerifyNoOtherCalls();
    }
}
