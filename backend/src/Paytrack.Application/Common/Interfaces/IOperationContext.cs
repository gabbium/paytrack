namespace Paytrack.Application.Common.Interfaces;

public interface IOperationContext
{
    string CorrelationId { get; }
    bool IsAuthenticated { get; }
    Guid? UserId { get; }
    Guid UserIdOrEmpty { get; }
}
