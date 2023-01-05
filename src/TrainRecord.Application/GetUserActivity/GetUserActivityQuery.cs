using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Application.GetUserActivity;

public class GetUserActivityQuery : IRequest<ErrorOr<IEnumerable<UserActivity>>>
{
    public Guid UserId { get; init; }
}

public class GetUserActivityQueryHandler
    : IRequestHandler<GetUserActivityQuery, ErrorOr<IEnumerable<UserActivity>>>
{
    private readonly DbSet<UserActivity> _userActivityDbSet;
    public AppDbContext _context { get; }

    public GetUserActivityQueryHandler(AppDbContext context)
    {
        _context = context;
        _userActivityDbSet = context.Set<UserActivity>();
    }

    public async Task<ErrorOr<IEnumerable<UserActivity>>> Handle(
        GetUserActivityQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _userActivityDbSet.Where(ua => ua.UserId == request.UserId).ToListAsync();
    }
}
