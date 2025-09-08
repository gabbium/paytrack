using Paytrack.Application.Common.Interfaces;

namespace Paytrack.Infrastructure.Data.Outbox;

internal sealed class OutboxInterceptor(
    IOperationContext operationContext,
    IOutboxSerializer outboxSerializer)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        EnqueueOutbox(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        EnqueueOutbox(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void EnqueueOutbox(DbContext? context)
    {
        if (context is not AppDbContext appContext)
            return;

        var entities = appContext.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var entity in entities)
            entity.ClearDomainEvents();

        var headers = new
        {
            userId = operationContext.UserId,
            correlationId = operationContext.CorrelationId,
            source = "paytrack.api"
        };

        var outboxMessages = domainEvents.Select(de =>
            new OutboxMessage(
                de.GetType().Name,
                outboxSerializer.Serialize(de),
                outboxSerializer.Serialize(headers),
                de.Timestamp
            )).ToList();

        appContext.OutboxMessages.AddRange(outboxMessages);
    }
}
