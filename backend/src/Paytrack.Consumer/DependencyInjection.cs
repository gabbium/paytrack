using Paytrack.Consumer.Workers;

namespace Paytrack.Consumer;

public static class DependencyInjection
{
    public static void AddConsumerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<ConsumerWorker>();
    }
}

