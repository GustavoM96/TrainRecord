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

namespace TrainRecord.Application.CreateUserActivity;

public class CreateUserActivityCommand : IRequest<ErrorOr<UserActivity>>
{
    public Guid UserId { get; init; }
    public Guid ActivityId { get; init; }
    public int Weight { get; init; }
    public int Repetition { get; init; }
}

public class CreateUserActivityCommandHandler
    : IRequestHandler<CreateUserActivityCommand, ErrorOr<UserActivity>>
{
    private readonly DbSet<UserActivity> _userActivityDbSet;
    public AppDbContext _context { get; }

    public CreateUserActivityCommandHandler(AppDbContext context)
    {
        _context = context;
        _userActivityDbSet = context.Set<UserActivity>();
    }

    public async Task<ErrorOr<UserActivity>> Handle(
        CreateUserActivityCommand request,
        CancellationToken cancellationToken
    )
    {
        var newActivity = request.Adapt<UserActivity>();

        await _userActivityDbSet.AddAsync(newActivity);
        await _context.SaveChangesAsync();

        return newActivity;
    }
}
