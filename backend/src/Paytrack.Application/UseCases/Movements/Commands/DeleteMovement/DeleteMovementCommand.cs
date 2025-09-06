namespace Paytrack.Application.UseCases.Movements.Commands.DeleteMovement;

public sealed record DeleteMovementCommand(
    Guid Id)
    : ICommand;
