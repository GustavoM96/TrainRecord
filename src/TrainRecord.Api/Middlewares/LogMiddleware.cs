using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using TrainRecord.Core.Extentions;

namespace TrainRecord.Api.Middlewares;

public class LogMiddleware
{
    private readonly Stopwatch _timer;
    private readonly RequestDelegate _next;
    private readonly ILogger<LogMiddleware> _logger;

    public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task Invoke(HttpContext context)
    {
        var elapsed = await _timer.GetTimeAsync(() => _next(context));

        var request = context.Request;
        request.Body.Position = 0;

        var bodyContent = await new StreamReader(request.Body).ReadToEndAsync();
        var path = request.Path;

        var statusCode = context.Response.StatusCode;
        var traceId = Activity.Current?.Id;

        _logger.LogInformation(
            "TrainRecord Long Running: {path} BodyContent: {bodyContent} Elapsed: {elapsed} StatusCode: {statusCode} TraceId: {traceId}",
            path,
            bodyContent,
            elapsed,
            statusCode,
            traceId
        );

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is not null)
        {
            _logger.LogError(exception, exception?.Message + "On TraceId: {traceId}", traceId);
        }
    }
}
