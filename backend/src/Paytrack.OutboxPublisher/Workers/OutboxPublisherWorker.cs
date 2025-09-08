using Paytrack.Infrastructure.Data;
using Paytrack.OutboxPublisher.Infrastructure;

namespace Paytrack.OutboxPublisher.Workers;

internal sealed class OutboxPublisherWorker(
    IServiceProvider serviceProvider,
    ILogger<OutboxPublisherWorker> logger)
    : BackgroundService
{
    private readonly int _batchSize = 200;
    private readonly int _pollIntervalMs = 10000;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<IBusPublisher>();

            var outboxMessages = await context.OutboxMessages
                .OrderBy(m => m.Timestamp)
                .Take(_batchSize)
                .ToListAsync(cancellationToken);

            if (outboxMessages.Count == 0)
            {
                logger.LogInformation("No outbox messages found");
                await Task.Delay(_pollIntervalMs, cancellationToken);
                continue;
            }

            foreach (var outboxMessage in outboxMessages)
            {
                using (LogContext.PushProperty("OutboxMessageId", outboxMessage.Id))
                using (LogContext.PushProperty("EventType", outboxMessage.Type))
                {
                    try
                    {
                        await publisher.PublishAsync(
                            outboxMessage.Type,
                            outboxMessage.Payload,
                            outboxMessage.Headers,
                            outboxMessage.Id,
                            cancellationToken);

                        context.OutboxMessages.Remove(outboxMessage);

                        logger.LogInformation("Successfully published outbox message");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to publish outbox message");
                    }
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            await Task.Delay(_pollIntervalMs, cancellationToken);
        }
    }
}
