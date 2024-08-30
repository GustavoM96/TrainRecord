using System.Text.Json;
using System.Text.Json.Serialization;
using LaDeak.JsonMergePatch.AspNetCore;
using LaDeak.JsonMergePatch.Generated.SafeApi;
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
    })
    .AddMvcOptions(options =>
    {
        LaDeak.JsonMergePatch.Abstractions.JsonMergePatchOptions.Repository =
            TypeRepository.Instance;
        options.InputFormatters.Insert(0, new JsonMergePatchInputReader());
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

app.Use(
    async (context, next) =>
    {
        context.Request.EnableBuffering();
        await next();
    }
);

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();

public class Program { }
