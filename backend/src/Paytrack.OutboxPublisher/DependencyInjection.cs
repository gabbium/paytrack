using Paytrack.Application.Common.Interfaces;
using Paytrack.OutboxPublisher.Infrastructure;
using Paytrack.OutboxPublisher.Services;
using Paytrack.OutboxPublisher.Workers;

namespace Paytrack.OutboxPublisher;

public static class DependencyInjection
{
    public static void AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOperationContext, OperationContext>();

        services
            .AddSingleton<IValidateOptions<RabbitMqOptions>, RabbitMqOptionsValidator>()
            .AddOptions<RabbitMqOptions>()
            .Bind(configuration.GetSection("RabbitMQ"))
            .ValidateOnStart();

        services.AddTransient<IBusPublisher, RabbitMqPublisher>();

        services.AddHostedService<OutboxPublisherWorker>();
    }
}

