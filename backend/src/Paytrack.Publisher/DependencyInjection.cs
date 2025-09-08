using Paytrack.Publisher.Workers;

namespace Paytrack.Publisher;

public static class DependencyInjection
{
    public static void AddPublisherServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<PublisherWorker>();
    }
}

