namespace Paytrack.Application.UseCases.Movements.Queries.GetMovementById;

internal sealed class GetMovementByIdValidator : AbstractValidator<GetMovementByIdQuery>
{
    public GetMovementByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
