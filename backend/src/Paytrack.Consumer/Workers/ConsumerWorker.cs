namespace Paytrack.Consumer.Workers;

public class ConsumerWorker(ILogger<ConsumerWorker> logger) : BackgroundService
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

        var queueName = "paytrack.all";

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(queueName, "paytrack", "movements.created", null, cancellationToken: stoppingToken);
        await channel.QueueBindAsync(queueName, "paytrack", "movements.updated", null, cancellationToken: stoppingToken);
        await channel.QueueBindAsync(queueName, "paytrack", "users.registered", null, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (ch, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            logger.LogInformation("Received {@Message}", message);
            await Task.CompletedTask;
        };

        var consumerTag = await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: true, 
            consumer: consumer,
            cancellationToken: stoppingToken);

        logger.LogInformation("Consumer started: {@ConsumerTag}", consumerTag);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            await channel.BasicCancelAsync(consumerTag, cancellationToken: stoppingToken);
            logger.LogInformation("Consumer stopped");
        }
    }
}
