using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.ActivityCommand;

public class DeleteAllRecordByUserActivityCommand : IRequest<ErrorOr<Deleted>>
{
    public Guid UserId { get; init; }
    public Guid ActivityId { get; init; }
}

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
