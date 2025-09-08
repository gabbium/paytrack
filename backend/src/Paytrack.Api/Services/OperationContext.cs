using Paytrack.Application.Common.Interfaces;

namespace Paytrack.Api.Services;

internal sealed class OperationContext : IOperationContext
{
    public Guid? UserId { get; internal set; }
    public bool IsAuthenticated { get; internal set; }
    public string CorrelationId { get; internal set; } = default!;
    public Guid UserIdOrEmpty => UserId ?? Guid.Empty;
}

