using Paytrack.Publisher;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((_, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration)
       .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
           .WithDefaultDestructurers()
           .WithDestructurers([new DbUpdateExceptionDestructurer()]));
});

builder.Services.AddPublisherServices(builder.Configuration);

var app = builder.Build();

await app.RunAsync();
