using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.ActivityCommand;

public class DeleteRecordCommand : IRequest<ErrorOr<Deleted>>
{
    public required EntityId<UserActivity> RecordId { get; init; }
}

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
        var deleted = await _userActivityRepository.DeleteById(request.RecordId);
        return deleted ? Result.Deleted : UserActivityErrors.NotFound;
    }
}
