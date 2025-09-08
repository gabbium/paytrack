namespace Paytrack.OutboxPublisher.Infrastructure;

public interface IBusPublisher
{
    Task PublishAsync(
        string subject,
        string payloadUtf8Json,
        string headersUtf8Json,
        Guid messageId,
        CancellationToken cancellationToken = default);
}
