using ErrorOr;
using MediatR;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.ActivityQuery;

public record GetAllActivityQuery(Pagination Pagination) : IRequest<ErrorOr<Page<Activity>>> { }

public class GetAllActivityQueryHandler
    : IRequestHandler<GetAllActivityQuery, ErrorOr<Page<Activity>>>
{
    private readonly IActivityRepository _activityRepository;

    public GetAllActivityQueryHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<ErrorOr<Page<Activity>>> Handle(
        GetAllActivityQuery request,
        CancellationToken cancellationToken
    )
    {
        return _activityRepository.AsPage(request.Pagination);
    }
}
