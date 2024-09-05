using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TrainRecord.Api;
using TrainRecord.Application;
using TrainRecord.Core;
using TrainRecord.Infrastructure;
using TrainRecord.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

services.AddProblemDetails();
services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddInfrastuctureServices(config);
services.AddApplicationServices();
services.AddApiServices();
services.AddCoreServices(config);
services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

var app = builder.Build();

app.MapHealthChecks(
    "/HealthChecks",
    new()
    {
        AllowCachingResponses = false,
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
        },
    }
);

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
if (!app.Environment.IsEnvironment("Test"))
{
    await AddMigrations();
}

app.Run();

async Task AddMigrations()
{
    await using var scope = app.Services.CreateAsyncScope();
    await DataBaseMigration.Migrate(scope.ServiceProvider);
}

public partial class Program { }
