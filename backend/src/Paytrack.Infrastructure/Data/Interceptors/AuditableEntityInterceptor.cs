using Paytrack.Application.Common.Interfaces;

namespace Paytrack.Infrastructure.Data.Interceptors;

internal sealed class AuditableEntityInterceptor(
    IOperationContext operationContext,
    TimeProvider dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var utcNow = dateTime.GetUtcNow();

        var entries = context.ChangeTracker
            .Entries<BaseAuditableEntity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified
                || entry.HasChangedOwnedEntities());

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = operationContext.UserId;
                entry.Entity.CreatedOn = utcNow;
            }

            entry.Entity.LastModifiedBy = operationContext.UserId;
            entry.Entity.LastModifiedOn = utcNow;
        }
    }
}

internal static class Extensions
{
    internal static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added
            || r.TargetEntry.State == EntityState.Modified));
}
