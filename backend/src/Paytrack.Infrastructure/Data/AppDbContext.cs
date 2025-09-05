using Paytrack.Application.Common.Interfaces;
using Paytrack.Domain.Entities;

namespace Paytrack.Infrastructure.Data;

internal sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    ICurrentUser currentUser) : DbContext(options), IUnitOfWork
{
    private readonly Guid _currentUserId = currentUser.UserId;

    public DbSet<User> Users => Set<User>();
    public DbSet<Movement> Movements => Set<Movement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Movement>()
            .HasQueryFilter(m => m.UserId == _currentUserId);

        base.OnModelCreating(modelBuilder);
    }
}
