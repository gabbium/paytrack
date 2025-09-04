using Paytrack.Infrastructure.Data;

namespace Paytrack.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<AppDbContextInitializer>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<ISender, Sender>();
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
    }
}
