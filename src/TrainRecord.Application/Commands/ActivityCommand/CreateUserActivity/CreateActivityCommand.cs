using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.ActivityCommand;

public class CreateUserActivityCommand : IRequest<ErrorOr<UserActivity>>
{
    public required EntityId<User> UserId { get; init; }
    public required EntityId<Activity> ActivityId { get; init; }
    public required int Weight { get; init; }
    public required int Repetition { get; init; }
    public required int Serie { get; init; }
}

public class CreateUserActivityCommandHandler
    : IRequestHandler<CreateUserActivityCommand, ErrorOr<UserActivity>>
{
    private readonly IUserActivityRepository _userActivityRepository;
    private readonly IUserRepository _userRepository;
    private readonly IActivityRepository _activityRepository;

    public CreateUserActivityCommandHandler(
        IUserActivityRepository userActivityRepository,
        IUserRepository userRepository,
        IActivityRepository activityRepository
    )
    {
        _userActivityRepository = userActivityRepository;
        _userRepository = userRepository;
        _activityRepository = activityRepository;
    }

    public async Task<ErrorOr<UserActivity>> Handle(
        CreateUserActivityCommand request,
        CancellationToken cancellationToken
    )
    {
        var newUserActivity = request.Adapt<UserActivity>();
        var hasUserAndActivityResult = await HasUserAndActivity(request);

        if (hasUserAndActivityResult.IsError)
        {
            return hasUserAndActivityResult.Errors;
        }

        await _userActivityRepository.AddAsync(newUserActivity);
        return newUserActivity;
    }

    private async Task<ErrorOr<Success>> HasUserAndActivity(CreateUserActivityCommand request)
    {
        var errors = new List<Error>();

        var anyUser = await _userRepository.AnyByIdAsync(request.UserId);
        if (!anyUser)
        {
            errors.Add(UserError.NotFound);
        }

        var anyActivity = await _activityRepository.AnyByIdAsync(request.ActivityId);
        if (!anyActivity)
        {
            errors.Add(ActivityErrors.NotFound);
        }

        return errors.Any() ? errors : Result.Success;
    }
}
