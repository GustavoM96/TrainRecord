using System.Collections.Immutable;
using LaDeak.JsonMergePatch.AspNetCore;
using LaDeak.JsonMergePatch.Generated;
using LaDeak.JsonMergePatch.Generated.SafeApi;
using TrainRecord.Api;
using TrainRecord.Api.Common.Policies.OwnerResourceRequirment;
using TrainRecord.Application;
using TrainRecord.Core;
using TrainRecord.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

// Add services to the container.

services
    .AddControllers()
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();
app.Run();
