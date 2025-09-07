namespace Paytrack.Api.FunctionalTests.Infrastructure.Support;

[Collection(FunctionalTestsCollection.Name)]
public abstract class TestBase(TestFixture fixture) : IAsyncLifetime
{
    protected readonly TestFixture Fixture = fixture;

    public async Task InitializeAsync()
    {
        await Fixture.ResetStateAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
