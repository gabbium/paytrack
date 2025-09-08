using Paytrack.Domain.Entities;
using Paytrack.Domain.Enums;

namespace Paytrack.Application.UseCases.Movements.Contracts;

public sealed record MovementResponse
{
    public Guid Id { get; init; }
    public MovementKind Kind { get; init; }
    public decimal Amount { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset OccurredOn { get; init; }

    public static MovementResponse FromDomain(Movement movement) =>
        new()
        {
            Id = movement.Id,
            Kind = movement.Kind,
            Amount = movement.Amount,
            Description = movement.Description,
            OccurredOn = movement.OccurredOn
        };
}
