using Paytrack.Application.UseCases.Movements.Queries.ListMovements;

namespace Paytrack.Application.UnitTests.TestData;

public sealed class ListMovementsQueryBuilder
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public ListMovementsQueryBuilder WithPageNumber(int pageNumber)
    {
        _pageNumber = pageNumber;
        return this;
    }

    public ListMovementsQueryBuilder WithPageSize(int pageSize)
    {
        _pageSize = pageSize;
        return this;
    }

    public ListMovementsQuery Build() => new(_pageNumber, _pageSize);
}
