using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUserService;

    public PerformanceBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
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
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsed = await _timer.GetTimeAsync(() => next());

        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? string.Empty;

        _logger.LogInformation(
            "TrainRecord Long Running: {Name} TimeSpan: {Elapsed} UserID: {@UserId} {@Request}",
            requestName,
            elapsed,
            userId,
            request
        );

        return response;
    }
}
