namespace Paytrack.Application.UseCases.Movements.Commands.DeleteMovement;

internal sealed class DeleteMovementCommandValidator : AbstractValidator<DeleteMovementCommand>
{
    public DeleteMovementCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
