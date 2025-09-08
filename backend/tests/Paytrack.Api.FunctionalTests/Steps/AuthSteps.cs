using Paytrack.Application.Common.Interfaces;

namespace Paytrack.Api.FunctionalTests.Steps;

public sealed class AuthSteps(TestFixture fx)
{
    public Task Given_LoggedInUser()
    {
        var user = new UserBuilder().Build();

        using var scope = fx.Factory.Services.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
        var accessToken = tokenService.CreateAccessToken(user);

        fx.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return Task.CompletedTask;
    }

    public Task Given_AnonymousUser()
    {
        fx.Client.DefaultRequestHeaders.Authorization = null;

        return Task.CompletedTask;
    }
}
