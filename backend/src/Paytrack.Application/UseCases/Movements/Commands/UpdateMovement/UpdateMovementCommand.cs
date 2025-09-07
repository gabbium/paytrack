using Paytrack.Application.UseCases.Movements.Contracts;
using Paytrack.Domain.Enums;

namespace Paytrack.Application.UseCases.Movements.Commands.UpdateMovement;

public sealed record UpdateMovementCommand(
    Guid Id,
    MovementKind Kind,
    decimal Amount,
    string? Description,
    DateTimeOffset OccurredOn)
    : ICommand<MovementResponse>;
