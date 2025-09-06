using Paytrack.Domain.Events;

namespace Paytrack.Application.UseCases.Users.Events;

internal sealed class UserRegisteredEventHandler(
    ILogger<UserRegisteredEventHandler> logger)
    : IDomainEventHandler<UserRegisteredEvent>
{
    public Task HandleAsync(UserRegisteredEvent @event, CancellationToken cancellationToken = default)
    {
        var name = @event.GetType().Name;

        logger.LogInformation("Handling domain event {DomainEventName} {@DomainEvent}", name, @event);

        return Task.CompletedTask;
    }
}
