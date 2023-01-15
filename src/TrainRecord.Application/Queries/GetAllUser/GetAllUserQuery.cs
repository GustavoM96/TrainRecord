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

namespace TrainRecord.Application.GetAllUserQuery;

public class GetAllUserQuery : IRequest<ErrorOr<Page<User>>>
{
    public Pagination Pagination { get; init; }
}

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, ErrorOr<Page<User>>>
{
    private readonly DbSet<User> _UserDbSet;
    public AppDbContext _context { get; }

    public GetAllUserQueryHandler(AppDbContext context)
    {
        _context = context;
        _UserDbSet = context.Set<User>();
    }

    public async Task<ErrorOr<Page<User>>> Handle(
        GetAllUserQuery request,
        CancellationToken cancellationToken
    )
    {
        return _UserDbSet.AsQueryable().Pagination(request.Pagination);
    }
}
