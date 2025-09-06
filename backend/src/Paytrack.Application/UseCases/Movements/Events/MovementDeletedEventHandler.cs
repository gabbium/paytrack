using Paytrack.Domain.Events;

namespace Paytrack.Application.UseCases.Movements.Events;

internal sealed class MovementDeletedEventHandler(
    ILogger<MovementDeletedEventHandler> logger)
    : IDomainEventHandler<MovementDeletedEvent>
{
    public Task HandleAsync(MovementDeletedEvent @event, CancellationToken cancellationToken = default)
    {
        var name = @event.GetType().Name;

        logger.LogInformation("Handling domain event {DomainEventName} {@DomainEvent}", name, @event);

        return Task.CompletedTask;
    }
}
