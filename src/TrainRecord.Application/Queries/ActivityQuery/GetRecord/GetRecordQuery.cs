using ErrorOr;
using MediatR;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.ActivityQuery;

public class GetRecordQuery : IRequest<ErrorOr<Page<UserActivity>>>
{
    public Guid UserId { get; init; }
    public Guid ActivityId { get; init; }
    public Pagination Pagination { get; init; }
}

public class GetRecordQueryHandler : IRequestHandler<GetRecordQuery, ErrorOr<Page<UserActivity>>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public GetRecordQueryHandler(IUserActivityRepository userActivityRepository)
    {
        _userActivityRepository = userActivityRepository;
    }

    public async Task<ErrorOr<Page<UserActivity>>> Handle(
        GetRecordQuery request,
        CancellationToken cancellationToken
    )
    {
        return _userActivityRepository
            .GetAllRecordByUserAndActivityId(request.UserId, request.ActivityId)
            .AsPage(request.Pagination);
    }
}
