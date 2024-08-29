using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.ActivityCommand;

public record DeleteRecordCommand(
    EntityId<UserActivity> RecordId,
    EntityId<User>? StudentId = null,
    EntityId<User>? TeacherId = null
) : IRequest<ErrorOr<Deleted>> { }

public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand, ErrorOr<Deleted>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public DeleteRecordCommandHandler(IUserActivityRepository userActivityRepository)
    {
        _userActivityRepository = userActivityRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteRecordCommand request,
        CancellationToken cancellationToken
    )
    {
        if (request.StudentId is not null)
        {
            var deleted = await _userActivityRepository.DeleteRecordByStudentId(
                request.RecordId,
                request.StudentId
            );
            return deleted ? Result.Deleted : UserActivityErrors.NotFound;
        }

        if (request.TeacherId is not null)
        {
            var deleted = await _userActivityRepository.DeleteRecordByTeacherId(
                request.RecordId,
                request.TeacherId
            );
            return deleted ? Result.Deleted : UserActivityErrors.NotFound;
        }

        return UserActivityErrors.NotFound;
    }
}
