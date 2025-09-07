using Paytrack.Api.Services;
using Paytrack.Domain.Enums;
using Paytrack.Infrastructure.Data;

namespace Paytrack.Api.FunctionalTests.Infrastructure.Containers;

public class PostgresContainer
{
    private readonly PostgreSqlContainer _container;
    private Respawner? _respawner;

    public PostgresContainer()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .Build();
    }

    public string ConnectionString => _container.GetConnectionString();

    public async Task StartAsync()
    {
        await _container.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSnakeCaseNamingConvention()
            .UseNpgsql(_container.GetConnectionString(), options =>
            {
                options.MapEnum<MovementKind>();
            })
            .Options;

        using var context = new AppDbContext(options, new OperationContext());

        await context.Database.MigrateAsync();

        await InitRespawnerAsync();
    }

    private async Task InitRespawnerAsync()
    {
        await using var conn = new NpgsqlConnection(ConnectionString);

        await conn.OpenAsync();

        _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = [new Table("__EFMigrationsHistory")]
        });
    }

    public async Task ResetAsync()
    {
        if (_respawner is null)
        {
            await InitRespawnerAsync();
        }

        await using var conn = new NpgsqlConnection(ConnectionString);

        await conn.OpenAsync();

        await _respawner!.ResetAsync(conn);
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
