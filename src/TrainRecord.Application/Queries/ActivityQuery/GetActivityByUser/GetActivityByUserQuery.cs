using ErrorOr;
using MediatR;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;

namespace TrainRecord.Application.ActivityQuery;

public record GetActivityByUserQuery(EntityId<User> UserId, Pagination Pagination)
    : IRequest<ErrorOr<Page<Activity>>> { }

public class GetActivityByUserQueryHandler
    : IRequestHandler<GetActivityByUserQuery, ErrorOr<Page<Activity>>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public GetActivityByUserQueryHandler(IUserActivityRepository userActivityRepository)
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
