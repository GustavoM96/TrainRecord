using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.ActivityCommand;

public record DeleteAllRecordByUserActivityCommand(
    EntityId<User> UserId,
    EntityId<Activity> ActivityId
) : IRequest<ErrorOr<Deleted>> { }

public class DeleteAllRecordByUserActivityCommandHandler
    : IRequestHandler<DeleteAllRecordByUserActivityCommand, ErrorOr<Deleted>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public DeleteAllRecordByUserActivityCommandHandler(
        IUserActivityRepository userActivityRepository
    )
    {
        _userActivityRepository = userActivityRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteAllRecordByUserActivityCommand request,
        CancellationToken cancellationToken
    )
    {
        var deleted = await _userActivityRepository.DeleteRecordByUserAndActivityId(
            request.UserId,
            request.ActivityId
        );

        return deleted ? Result.Deleted : UserActivityErrors.NotFound;
    }
}
