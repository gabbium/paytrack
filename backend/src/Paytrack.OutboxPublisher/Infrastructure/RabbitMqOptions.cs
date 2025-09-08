namespace Paytrack.OutboxPublisher.Infrastructure;

public sealed class RabbitMqOptions
{
    public string HostName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Exchange { get; set; } = default!;
}

public sealed class RabbitMqOptionsValidator : IValidateOptions<RabbitMqOptions>
{
    public ValidateOptionsResult Validate(string? name, RabbitMqOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.HostName))
            return ValidateOptionsResult.Fail("RabbitMQ hostname not configured");

        if (string.IsNullOrWhiteSpace(options.UserName))
            return ValidateOptionsResult.Fail("RabbitMQ username not configured");

        if (string.IsNullOrWhiteSpace(options.Password))
            return ValidateOptionsResult.Fail("RabbitMQ password not configured");

        if (string.IsNullOrWhiteSpace(options.Exchange))
            return ValidateOptionsResult.Fail("RabbitMQ exchange not configured");

        return ValidateOptionsResult.Success;
    }
}
