namespace Paytrack.Infrastructure.Data.Outbox;

public sealed class SystemTextJsonOutboxSerializer : IOutboxSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        Converters = 
        { 
            new JsonStringEnumConverter() 
        }
    };

    public string Serialize(object value) => JsonSerializer.Serialize(value, Options);
}
