namespace Paytrack.Application.UseCases.Movements.Commands.UpdateMovement;

internal sealed class UpdateMovementCommandValidator : AbstractValidator<UpdateMovementCommand>
{
    public UpdateMovementCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

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
