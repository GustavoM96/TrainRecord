using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using TrainRecord.Infrastructure.Interfaces;

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
        var requestName = typeof(TRequest).Name;
        var userId = string.Empty;

        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        _logger.LogWarning(
            "CleanArchitecture Long Running: {Name} Time: ({ElapsedMilliseconds} milliseconds) UserID: {@UserId} {@Request}",
            requestName,
            elapsedMilliseconds,
            userId,
            request
        );

        return response;
    }
}
