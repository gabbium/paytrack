namespace Paytrack.Application.UseCases.Movements.Queries.ListMovements;

internal sealed class ListMovementsQueryValidator : AbstractValidator<ListMovementsQuery>
{
    public ListMovementsQueryValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThan(0);

        RuleFor(q => q.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}
