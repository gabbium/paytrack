using Paytrack.Domain.Events;

namespace Paytrack.Application.UseCases.Movements.Events;

internal sealed class MovementUpdatedEventHandler(
    ILogger<MovementUpdatedEventHandler> logger)
    : IDomainEventHandler<MovementUpdatedEvent>
{
    public Task HandleAsync(MovementUpdatedEvent @event, CancellationToken cancellationToken = default)
    {
        var name = @event.GetType().Name;

        logger.LogInformation("Handling domain event {DomainEventName} {@DomainEvent}", name, @event);

        return Task.CompletedTask;
    }
}
