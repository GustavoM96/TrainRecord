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
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Application.GetUserActivity;

public class GetUserActivityQuery : IRequest<ErrorOr<Page<UserActivity>>>
{
    public Guid UserId { get; init; }
    public Pagination Pagination { get; init; }
}

public class GetUserActivityQueryHandler
    : IRequestHandler<GetUserActivityQuery, ErrorOr<Page<UserActivity>>>
{
    private readonly DbSet<UserActivity> _userActivityDbSet;
    public AppDbContext _context { get; }

    public GetUserActivityQueryHandler(AppDbContext context)
    {
        _context = context;
        _userActivityDbSet = context.Set<UserActivity>();
    }

    public async Task<ErrorOr<Page<UserActivity>>> Handle(
        GetUserActivityQuery request,
        CancellationToken cancellationToken
    )
    {
        return _userActivityDbSet
            .Where(ua => ua.UserId == request.UserId)
            .Pagination(request.Pagination);
    }
}
