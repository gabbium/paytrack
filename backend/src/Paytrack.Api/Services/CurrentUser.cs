using Paytrack.Application.Common.Interfaces;

namespace Paytrack.Api.Services;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool HasUser => User?.Identity?.IsAuthenticated == true;

    public Guid UserId => User.GetUserId();
}

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        if (principal is null) return Guid.Empty;

        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(value, out var userId) ? userId : Guid.Empty;
    }
}

