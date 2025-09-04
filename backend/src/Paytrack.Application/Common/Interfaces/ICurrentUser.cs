namespace Paytrack.Application.Common.Interfaces;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
}

