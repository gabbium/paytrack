using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Domain.Enums;

namespace Paytrack.Application.UseCases.Movements.Commands.CreateMovement;

public sealed record CreateMovementCommand(
    MovementKind Kind,
    decimal Amount,
    string? Description,
    DateTimeOffset OccurredOn)
    : ICommand<MovementResponse>;
