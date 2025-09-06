namespace Paytrack.Api.Infrastructure;

internal sealed class SensitiveDataDestructurer : IDestructuringPolicy
{
    private const string Mask = "******";

    public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
    {
        var type = value.GetType();
        var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        if (!props.Any(p => p.Name.Equals("Password", StringComparison.OrdinalIgnoreCase)))
        {
            result = null!;
            return false;
        }

        var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var prop in props)
        {
            if (!prop.CanRead) continue;

            object? valueToLog = prop.GetValue(value);

            if (prop.Name.Equals("Password", StringComparison.OrdinalIgnoreCase))
                valueToLog = Mask;

            dict[prop.Name] = valueToLog;
        }

        result = propertyValueFactory.CreatePropertyValue(dict, destructureObjects: true);

        return true;
    }
}

