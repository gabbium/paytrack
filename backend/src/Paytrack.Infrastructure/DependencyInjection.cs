using Paytrack.Application.Common.Interfaces;
using Paytrack.Application.UseCases.Movements.Queries.ListMovements;
using Paytrack.Domain.Enums;
using Paytrack.Domain.Repositories;
using Paytrack.Infrastructure.Data;
using Paytrack.Infrastructure.Data.Interceptors;
using Paytrack.Infrastructure.Data.Outbox;
using Paytrack.Infrastructure.Data.Queries;
using Paytrack.Infrastructure.Data.Repositories;
using Paytrack.Infrastructure.Messaging;
using Paytrack.Infrastructure.Security;

namespace Paytrack.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, string? connectionString)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, OutboxInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options
                .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                .UseSnakeCaseNamingConvention()
                .UseNpgsql(connectionString, options =>
                {
                    options.MapEnum<MovementKind>();
                });
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IMediator, Mediator>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenService, TokenService>();

        services.AddScoped<IOutboxSerializer, SystemTextJsonOutboxSerializer>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IMovementRepository, MovementRepository>();
        services.AddScoped<IListMovementsQueryService, ListMovementsQueryService>();
    }
}
