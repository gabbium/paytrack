namespace Paytrack.Infrastructure.Data.Outbox;

public sealed class OutboxMessage(string type, string payload, string headers, DateTimeOffset timestamp)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = type;
    public string Payload { get; set; } = payload;
    public string Headers { get; init; } = headers;
    public DateTimeOffset Timestamp { get; set; } = timestamp;
}
