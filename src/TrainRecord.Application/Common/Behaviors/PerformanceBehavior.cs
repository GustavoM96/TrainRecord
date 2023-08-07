using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUserService;

    public PerformanceBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var result = await _timer.GetTimeAsync(() => next());

        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? string.Empty;

        _logger.LogInformation(
            "TrainRecord Long Running: {Name} TimeSpan: {Elapsed} UserID: {@UserId} {@Request}",
            requestName,
            result.Elapsed,
            userId,
            request
        );

        return result.Value;
    }
}
