using Paytrack.Infrastructure;
using Paytrack.OutboxPublisher;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((_, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration)
       .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
           .WithDefaultDestructurers()
           .WithDestructurers([new DbUpdateExceptionDestructurer()]));
});

builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddWorkerServices(builder.Configuration);

var app = builder.Build();

await app.RunAsync();
