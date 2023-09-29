using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Events.ActivityEvents;
using TrainRecord.Core.Entities;
using TrainRecord.Application.Interfaces.Repositories;

namespace TrainRecord.Application.ActivityCommand;

public record CreateActivityCommand(string Name) : IRequest<ErrorOr<Activity>> { }

public class CreateActivityCommandHandler
    : IRequestHandler<CreateActivityCommand, ErrorOr<Activity>>
{
    private readonly IActivityRepository _activityRepository;

    public CreateActivityCommandHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<ErrorOr<Activity>> Handle(
        CreateActivityCommand request,
        CancellationToken cancellationToken
    )
    {
        var newActivity = request.Adapt<Activity>();

        var anyActivity = await _activityRepository.AnyByNameAsync(request.Name);
        if (anyActivity)
        {
            return ActivityErrors.NameAlreadyExists;
        }

        newActivity.AddDomainEvent(new CreateActivityEvent() { ActivityName = newActivity.Name });
        await _activityRepository.AddAsync(newActivity);
        return newActivity;
    }
}
