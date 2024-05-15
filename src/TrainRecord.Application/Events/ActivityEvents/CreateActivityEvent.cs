using MediatR;
using Microsoft.Extensions.Logging;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.Events.ActivityEvents;

public record CreateActivityEvent(string ActivityName) : IDomainEvent<Activity> { }

public class CreateActivityEventHandle : INotificationHandler<CreateActivityEvent>
{
    private readonly ILogger<CreateActivityEventHandle> _logger;

    public CreateActivityEventHandle(ILogger<CreateActivityEventHandle> logger)
    {
        _logger = logger;
    }

    public Task Handle(CreateActivityEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Notification new activity {activityName}",
            notification.ActivityName
        );
        return Task.CompletedTask;
    }
}
