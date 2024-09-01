using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Builders;
using TrainRecord.Core.Commum.Bases;

namespace Api.Common.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(
            exception,
            ProblemDetailsBuilder.UnhandledExceptionMsg + "TraceId: {traceId}",
            context.TraceIdentifier
        );

        var problemDetails = exception is HandlerException handlerException
            ? ProblemDetailsBuilder.Build(context.TraceIdentifier, handlerException.Errors)
            : ProblemDetailsBuilder.Build(context.TraceIdentifier, exception);

        var json = ToJson(problemDetails);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetails.Status!.Value;

        await context.Response.WriteAsync(json, cancellationToken);
        return true;
    }

    private string ToJson(ProblemDetails problemDetails)
    {
        try
        {
            return JsonSerializer.Serialize(problemDetails, SerializerOptions);
        }
        catch (Exception ex)
        {
            const string message = "An exception has occurred while serializing error to JSON.";
            _logger.LogError(ex, message);
            return message;
        }
    }
}
