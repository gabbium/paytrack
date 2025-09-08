using Paytrack.Domain.Resources;

namespace Paytrack.Application.UseCases.Movements.Commands.CreateMovement;

internal sealed class CreateMovementCommandValidator : AbstractValidator<CreateMovementCommand>
{
    public CreateMovementCommandValidator()
    {
        RuleFor(c => c.Amount)
            .GreaterThan(0m)
            .WithMessage(string.Format(Resource.Movement_Amount_GreaterThan, 0))
            .PrecisionScale(18, 2, true)
            .WithMessage(string.Format(Resource.Movement_Amount_PrecisionScale, 18, 2));

        RuleFor(c => c.Description)
            .MaximumLength(128)
            .WithMessage(string.Format(Resource.Movement_Description_MaxLength, 128));
    }
}
