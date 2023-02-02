using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.GetActivityByUserQuery;

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
