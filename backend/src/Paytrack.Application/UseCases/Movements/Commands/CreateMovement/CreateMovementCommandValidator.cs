namespace Paytrack.Application.UseCases.Movements.Commands.CreateMovement;

internal sealed class CreateMovementCommandValidator : AbstractValidator<CreateMovementCommand>
{
    public CreateMovementCommandValidator()
    {
        RuleFor(c => c.Kind)
            .IsInEnum();

        RuleFor(c => c.Amount)
            .GreaterThan(0m)
            .PrecisionScale(18, 2, true);

        RuleFor(c => c.OccurredOn);

        RuleFor(c => c.Description)
            .MaximumLength(128);
    }
}
