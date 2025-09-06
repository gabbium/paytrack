using Paytrack.Domain.Entities;

namespace Paytrack.Application.Common.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(User user);
}
