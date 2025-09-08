using Paytrack.Domain.Entities;

namespace Paytrack.Domain.Repositories;

public interface IMovementRepository
{
    Task<Movement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Movement movement, CancellationToken cancellationToken = default);
    Task UpdateAsync(Movement movement, CancellationToken cancellationToken = default);
    Task RemoveAsync(Movement movement, CancellationToken cancellationToken = default);
}
