using ErrorOr;
using MediatR;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extensions;

namespace TrainRecord.Application.ActivityQuery;

public record GetRecordByActivityIdQuery(
    EntityId<User> UserId,
    EntityId<Activity> ActivityId,
    Pagination Pagination
) : IRequest<ErrorOr<Page<UserActivity>>> { }

public class GetRecordQueryHandler
    : IRequestHandler<GetRecordByActivityIdQuery, ErrorOr<Page<UserActivity>>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public GetRecordQueryHandler(IUserActivityRepository userActivityRepository)
    {
        _userActivityRepository = userActivityRepository;
    }

    public async Task<ErrorOr<Page<UserActivity>>> Handle(
        GetRecordByActivityIdQuery request,
        CancellationToken cancellationToken
    )
    {
        return _userActivityRepository
            .GetAllRecordByUserAndActivityId(request.UserId, request.ActivityId)
            .AsPage(request.Pagination);
    }
}
