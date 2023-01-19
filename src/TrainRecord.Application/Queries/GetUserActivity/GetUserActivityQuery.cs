using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Common.Extentions;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.GetUserActivity;

public class GetUserActivityQuery : IRequest<ErrorOr<Page<UserActivity>>>
{
    public Guid UserId { get; init; }
    public Pagination Pagination { get; init; }
}

public class GetUserActivityQueryHandler
    : IRequestHandler<GetUserActivityQuery, ErrorOr<Page<UserActivity>>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public GetUserActivityQueryHandler(IUserActivityRepository userActivityRepository)
    {
        _userActivityRepository = userActivityRepository;
    }

    public async Task<ErrorOr<Page<UserActivity>>> Handle(
        GetUserActivityQuery request,
        CancellationToken cancellationToken
    )
    {
        return _userActivityRepository.GetAllByUserId(request.UserId).AsPage(request.Pagination);
    }
}
