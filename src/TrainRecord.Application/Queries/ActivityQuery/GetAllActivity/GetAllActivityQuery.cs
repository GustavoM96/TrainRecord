using ErrorOr;
using MediatR;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.ActivityQuery;

public class GetAllActivityQuery : IRequest<ErrorOr<Page<Activity>>>
{
    public required Pagination Pagination { get; init; }
}

public class GetAllActivityQueryHandler
    : IRequestHandler<GetAllActivityQuery, ErrorOr<Page<Activity>>>
{
    public readonly IActivityRepository _activityRepository;

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
