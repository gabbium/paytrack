namespace Paytrack.Application.Common.Interfaces;

public interface ICurrentUser
{
    bool HasUser { get; }
    Guid UserId { get; }
}

