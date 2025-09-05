using Paytrack.Application.Common.Interfaces;

namespace Paytrack.Infrastructure.Data.Interceptors;

public sealed class AuditableEntityInterceptor(ICurrentUser currentUser, TimeProvider dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var utcNow = dateTime.GetUtcNow();
        var actorId = currentUser.IsAuthenticated ? currentUser.UserId.ToString() : null;

        var entries = context.ChangeTracker
            .Entries<BaseAuditableEntity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities());

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = actorId;
                entry.Entity.CreatedOn = utcNow;
            }

            entry.Entity.LastModifiedBy = actorId;
            entry.Entity.LastModifiedOn = utcNow;
        }
    }
}


public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
