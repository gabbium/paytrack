using Paytrack.Api;
using Paytrack.Api.Infrastructure;
using Paytrack.Application;
using Paytrack.Infrastructure;
using Paytrack.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration)
       .Enrich.FromLogContext()
       .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
           .WithDefaultDestructurers()
           .WithDestructurers([new DbUpdateExceptionDestructurer()]));
});

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddWebServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<RequestContextLoggingMiddleware>();

app.UseExceptionHandler();

app.UseSerilogRequestLogging(options =>
{
    options.IncludeQueryInRequestPath = true;
});

app.UseOpenApi(settings =>
{
    settings.Path = "/api/specification.json";
});

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapHealthChecks("api/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints(Assembly.GetExecutingAssembly());

if (app.Environment.IsDevelopment())
{
    await app.Services.InitializeDatabaseAsync();
}

await app.RunAsync();
