using Paytrack.Api.FunctionalTests.Infrastructure.Containers;
using Paytrack.Api.FunctionalTests.Infrastructure.Hosting;
using Paytrack.Application.Common.Interfaces;

namespace Paytrack.Api.FunctionalTests.Infrastructure.Support;

public class TestFixture : IAsyncLifetime
{
    public PostgresContainer Database { get; private set; } = null!;
    public CustomWebApplicationFactory Factory { get; private set; } = null!;
    public HttpClient Client { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        Database = new PostgresContainer();
        await Database.StartAsync();

        Factory = new CustomWebApplicationFactory(Database.ConnectionString);
        Client = Factory.CreateClient();
    }

    public Task RunAsDefaultUserAsync()
    {
        return RunAsUserAsync("test@local");
    }

    public Task RunAsUserAsync(string email)
    {
        using var scope = Factory.Services.CreateScope();

        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        var user = new UserBuilder()
            .WithEmail(email)
            .Build();

        var accessToken = tokenService.CreateAccessToken(user);

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return Task.CompletedTask;
    }

    public async Task ResetStateAsync()
    {
        await Database.ResetAsync();
        Client = Factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
        await Database.DisposeAsync();
    }
}
