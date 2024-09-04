using System.Text.Json;
using System.Text.Json.Serialization;
using TrainRecord.Api;
using TrainRecord.Application;
using TrainRecord.Core;
using TrainRecord.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

// Add services to the container.

services.AddProblemDetails();
services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddInfrastuctureServices(config);
services.AddApplicationServices();
services.AddApiServices();
services.AddCoreServices(config);

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
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
    AddMigrations().Wait();
}

app.Run();

async Task AddMigrations()
{
    await using var scope = app.Services.CreateAsyncScope();
    await DataBaseMigration.Migrate(scope.ServiceProvider);
}

public partial class Program { }
