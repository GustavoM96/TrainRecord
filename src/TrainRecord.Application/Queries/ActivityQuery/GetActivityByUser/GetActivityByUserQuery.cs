using ErrorOr;
using MediatR;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.ActivityQuery;

public class GetActivityByUserQuery : IRequest<ErrorOr<Page<Activity>>>
{
    public Guid UserId { get; init; }
    public Pagination Pagination { get; init; }
}

public class GetUserActivityQueryHandler
    : IRequestHandler<GetActivityByUserQuery, ErrorOr<Page<Activity>>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public GetUserActivityQueryHandler(IUserActivityRepository userActivityRepository)
    {
        _userActivityRepository = userActivityRepository;
    }

    public async Task<ErrorOr<Page<Activity>>> Handle(
        GetActivityByUserQuery request,
        CancellationToken cancellationToken
    )
    {
        return _userActivityRepository
            .GetActivitiesByUserId(request.UserId)
            .AsPage(request.Pagination);
    }
}
