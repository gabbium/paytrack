using Paytrack.Domain.Resources;

namespace Paytrack.Application.UseCases.Movements.Queries.GetMovementById;

internal sealed class GetMovementByIdValidator : AbstractValidator<GetMovementByIdQuery>
{
    public GetMovementByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty()
            .WithMessage(Resource.Movement_Id_NotEmpty);
    }
}
