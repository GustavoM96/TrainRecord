using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.ActivityCommand;

public class CreateActivityCommand : IRequest<ErrorOr<Activity>>
{
    public required string Name { get; init; }
}

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

        await _activityRepository.AddAsync(newActivity);
        return newActivity;
    }
}
