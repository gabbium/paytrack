namespace Paytrack.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.Scan(s => s.FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces().WithScopedLifetime()
        );

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(ValidationBehavior.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(ValidationBehavior.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(ValidationBehavior.CommandHandler<>));

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingBehavior.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(LoggingBehavior.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingBehavior.CommandHandler<>));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}
