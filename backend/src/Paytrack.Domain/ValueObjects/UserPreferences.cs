namespace Paytrack.Domain.ValueObjects;

public sealed record UserPreferences(string Currency, string TimeZone)
{
    public static UserPreferences Default() => new("BRL", "America/Sao_Paulo");
}
