using Paytrack.Domain.Events;

namespace Paytrack.Application.UseCases.Movements.Events;

internal sealed class MovementCreatedEventHandler(
    ILogger<MovementCreatedEventHandler> logger)
    : IDomainEventHandler<MovementCreatedEvent>
{
    public Task HandleAsync(MovementCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        var name = @event.GetType().Name;

        logger.LogInformation("Handling domain event {DomainEventName} {@DomainEvent}", name, @event);

        return Task.CompletedTask;
    }
}
