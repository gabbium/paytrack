using Paytrack.Application.Common.Interfaces;
using Paytrack.Domain.Enums;
using Paytrack.Infrastructure.Data;
using Paytrack.Infrastructure.Data.Interceptors;
using Paytrack.Infrastructure.Security;

namespace Paytrack.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, string? connectionString)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString, options =>
            {
                options.MapEnum<MovementKind>();
            })
            .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddSingleton(TimeProvider.System);

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenService, TokenService>();

        services.AddScoped<ISender, Sender>();
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
    }
}
