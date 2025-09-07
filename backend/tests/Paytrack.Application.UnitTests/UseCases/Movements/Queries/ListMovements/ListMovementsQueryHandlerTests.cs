using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Application.UseCases.Movements.Queries.ListMovements;

namespace Paytrack.Application.UnitTests.UseCases.Movements.Queries.ListMovements;

public class ListMovementsQueryHandlerTests
{
    private readonly Mock<IListMovementsQueryService> _listMovementsQueryServiceMock;
    private readonly ListMovementsQueryHandler _handler;

    public ListMovementsQueryHandlerTests()
    {
        _listMovementsQueryServiceMock = new Mock<IListMovementsQueryService>();
        _handler = new ListMovementsQueryHandler(
            _listMovementsQueryServiceMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess()
    {
        // Arrange
        var query = new ListMovementsQueryBuilder().Build();
        var paginatedList = new PaginatedList<MovementResponse>(
            [],
            100,
            query.PageNumber,
            query.PageSize);

        _listMovementsQueryServiceMock
            .Setup(s => s.ListAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedList);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(paginatedList);

        _listMovementsQueryServiceMock.Verify(s =>
            s.ListAsync(query, It.IsAny<CancellationToken>()), Times.Once);

        _listMovementsQueryServiceMock.VerifyNoOtherCalls();
    }
}
