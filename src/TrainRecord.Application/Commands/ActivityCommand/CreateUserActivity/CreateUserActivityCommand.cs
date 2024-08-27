using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.ActivityCommand;

public record CreateUserActivityCommand(
    EntityId<User> UserId,
    EntityId<User>? TeacherId,
    EntityId<Activity> ActivityId,
    int Weight,
    int Repetition,
    int Serie,
    string? TrainGroup,
    string? TrainName,
    TimeOnly? Time
) : IRequest<ErrorOr<UserActivity>> { }

public class CreateUserActivityCommandHandler
    : IRequestHandler<CreateUserActivityCommand, ErrorOr<UserActivity>>
{
    private readonly IUserActivityRepository _userActivityRepository;
    private readonly IUserRepository _userRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly ITeacherStudentRepository _teacherStudentRepository;

    public CreateUserActivityCommandHandler(
        IUserActivityRepository userActivityRepository,
        IUserRepository userRepository,
        IActivityRepository activityRepository,
        ITeacherStudentRepository teacherStudentRepository
    )
    {
        _userActivityRepository = userActivityRepository;
        _userRepository = userRepository;
        _activityRepository = activityRepository;
        _teacherStudentRepository = teacherStudentRepository;
    }

    public async Task<ErrorOr<UserActivity>> Handle(
        CreateUserActivityCommand request,
        CancellationToken cancellationToken
    )
    {
        var newUserActivity = request.Adapt<UserActivity>();
        var hasUserAndActivityResult = await ExistsUserAndActivity(request);

        if (hasUserAndActivityResult.IsError)
        {
            return hasUserAndActivityResult.Errors;
        }

        await _userActivityRepository.AddAsync(newUserActivity);
        return newUserActivity;
    }

    private async Task<ErrorOr<Success>> ExistsUserAndActivity(CreateUserActivityCommand request)
    {
        var errors = new List<Error>();

        var anyUser = await _userRepository.AnyByIdAsync(request.UserId);
        if (!anyUser)
        {
            errors.Add(UserError.NotFound);
        }

        if (
            request.TeacherId is not null
            && !await _teacherStudentRepository.IsTeacherStudent(request.UserId, request.TeacherId)
        )
        {
            errors.Add(UserError.TeacherStudentNotFound);
        }

        var anyActivity = await _activityRepository.AnyByIdAsync(request.ActivityId);
        if (!anyActivity)
        {
            errors.Add(ActivityErrors.NotFound);
        }

        return errors.Count != 0 ? errors : Result.Success;
    }
}
