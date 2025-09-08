namespace Paytrack.Api.FunctionalTests.Infrastructure.Support;

public static class TestConstants
{
    public static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };
}
