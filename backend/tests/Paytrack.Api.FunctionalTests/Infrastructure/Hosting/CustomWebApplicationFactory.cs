namespace Paytrack.Api.FunctionalTests.Infrastructure.Hosting;

public class CustomWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", connectionString);
        builder.UseSetting("Jwt:Secret", Guid.NewGuid().ToString());
    }
}

