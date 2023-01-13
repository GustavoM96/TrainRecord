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
using TrainRecord.Application.GetAllActivity;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Application.GetAllActivity;

public class GetAllActivityQuery : Pagination, IRequest<ErrorOr<Page<Activity>>> { }

public class GetAllActivityQueryHandler
    : IRequestHandler<GetAllActivityQuery, ErrorOr<Page<Activity>>>
{
    private readonly DbSet<Activity> _activityDbSet;
    public AppDbContext _context { get; }

    public GetAllActivityQueryHandler(AppDbContext context)
    {
        _context = context;
        _activityDbSet = context.Set<Activity>();
    }

    public async Task<ErrorOr<Page<Activity>>> Handle(
        GetAllActivityQuery request,
        CancellationToken cancellationToken
    )
    {
        return _activityDbSet.AsQueryable().Pagination(request);
    }
}
