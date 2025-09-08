using Paytrack.Infrastructure.Data;

namespace Paytrack.Publisher.Workers;

public class PublisherWorker(
    IServiceProvider serviceProvider,
    ILogger<PublisherWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        using var connection = await connectionFactory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(null, stoppingToken);

        await channel.ExchangeDeclareAsync(
            exchange: "paytrack",
            type: ExchangeType.Topic,
            durable: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var outboxMessages = await context.OutboxMessages
                .OrderBy(m => m.Timestamp)
                .Take(10)
                .ToListAsync(stoppingToken);

            if (outboxMessages.Count == 0)
            {
                logger.LogInformation("No outbox messages found");
                await Task.Delay(1000, stoppingToken);
                continue;
            }

            foreach (var outboxMessage in outboxMessages)
            {
                var props = new BasicProperties
                {
                    ContentType = "application/json",
                    DeliveryMode = DeliveryModes.Persistent,
                    MessageId = outboxMessage.Id.ToString(),
                    Type = outboxMessage.Type,
                    Headers = new Dictionary<string, object?>
                    {
                        { "x-headers-json", outboxMessage.Headers }
                    }
                };

                var payload = Encoding.UTF8.GetBytes(outboxMessage.Payload);

                await channel.BasicPublishAsync(
                    exchange: "paytrack",
                    routingKey: outboxMessage.Type,
                    mandatory: false,
                    basicProperties: props,
                    body: payload,
                    cancellationToken: stoppingToken);

                context.OutboxMessages.Remove(outboxMessage);

                logger.LogInformation("Published {@Message}", outboxMessage);
            }

            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
