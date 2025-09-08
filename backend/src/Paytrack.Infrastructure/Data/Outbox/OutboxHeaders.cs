namespace Paytrack.Infrastructure.Data.Outbox;

public sealed record OutboxHeaders(
    Guid? UserId,
    string? CorrelationId,
    string Source
);
