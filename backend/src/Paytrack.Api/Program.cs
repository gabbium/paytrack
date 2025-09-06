using Paytrack.Api;
using Paytrack.Api.Infrastructure;
using Paytrack.Application;
using Paytrack.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration)
       .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
           .WithDefaultDestructurers()
           .WithDestructurers([new DbUpdateExceptionDestructurer()]))
       .Destructure.With(new SensitiveDataDestructurer());
});

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddWebServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<RequestContextLoggingMiddleware>();

app.UseSerilogRequestLogging(options =>
{
    options.IncludeQueryInRequestPath = true;
});

app.UseExceptionHandler();

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
}).DisableHttpMetrics();

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints(Assembly.GetExecutingAssembly());

await app.RunAsync();
