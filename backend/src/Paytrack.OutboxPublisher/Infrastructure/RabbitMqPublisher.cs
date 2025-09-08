namespace Paytrack.OutboxPublisher.Infrastructure;

internal sealed class RabbitMqPublisher(IOptions<RabbitMqOptions> rabbitOptions) : IBusPublisher
{
    private readonly RabbitMqOptions _rabbitOptions = rabbitOptions.Value;

    public async Task PublishAsync(
        string subject,
        string payloadUtf8Json,
        string headersUtf8Json,
        Guid messageId,
        CancellationToken cancellationToken = default)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitOptions.HostName,
            UserName = _rabbitOptions.UserName,
            Password = _rabbitOptions.Password
        };

        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using var channel = await connection.CreateChannelAsync(null, cancellationToken);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = messageId.ToString(),
            Type = subject,
            Headers = new Dictionary<string, object?>
            {
                { "x-headers-json", headersUtf8Json }
            }
        };

        var body = Encoding.UTF8.GetBytes(payloadUtf8Json);

        await channel.BasicPublishAsync(
            exchange:_rabbitOptions.Exchange,
            routingKey: subject,
            false,
            props,
            body,
            cancellationToken);
    }
}
