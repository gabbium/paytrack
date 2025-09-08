using Paytrack.Domain.Resources;

namespace Paytrack.Application.UseCases.Movements.Queries.ListMovements;

internal sealed class ListMovementsQueryValidator : AbstractValidator<ListMovementsQuery>
{
    public ListMovementsQueryValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.Movement_PageNumber_GreaterThan, 0));

        RuleFor(q => q.PageSize)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.Movement_PageSize_GreaterThan, 0))
            .LessThanOrEqualTo(100)
            .WithMessage(string.Format(Resource.Movement_PageSize_Max, 100));
    }
}
