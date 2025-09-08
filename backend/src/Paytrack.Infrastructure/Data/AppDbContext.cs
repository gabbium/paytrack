using Paytrack.Application.Common.Interfaces;
using Paytrack.Domain.Entities;
using Paytrack.Infrastructure.Data.Outbox;

namespace Paytrack.Infrastructure.Data;

internal sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IOperationContext operationContext)
    : DbContext(options), IUnitOfWork
{
    private readonly Guid _currentUserId = operationContext.UserIdOrEmpty;

    public DbSet<User> Users => Set<User>();
    public DbSet<Movement> Movements => Set<Movement>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Movement>()
            .HasQueryFilter(m => m.UserId == _currentUserId);

        base.OnModelCreating(modelBuilder);
    }
}
