using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Application.GetAllActivity;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.GetAllActivity;

public class GetAllActivityQuery : IRequest<ErrorOr<Page<Activity>>>
{
    public Pagination Pagination { get; init; }
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
