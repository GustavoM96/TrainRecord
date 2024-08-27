using ErrorOr;
using MediatR;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extensions;

namespace TrainRecord.Application.ActivityQuery;

public record GetAllRecordQuery(
    EntityId<User> UserId,
    EntityId<User>? TeacherId,
    Pagination Pagination
) : IRequest<ErrorOr<Page<UserActivity>>> { }

public class GetAllRecordQueryHandler
    : IRequestHandler<GetAllRecordQuery, ErrorOr<Page<UserActivity>>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public GetAllRecordQueryHandler(IUserActivityRepository userActivityRepository)
    {
        _userActivityRepository = userActivityRepository;
    }

    public async Task<ErrorOr<Page<UserActivity>>> Handle(
        GetAllRecordQuery request,
        CancellationToken cancellationToken
    )
    {
        return _userActivityRepository
            .GetAllRecordByUser(request.UserId, request.TeacherId)
            .AsPage(request.Pagination);
    }
}
