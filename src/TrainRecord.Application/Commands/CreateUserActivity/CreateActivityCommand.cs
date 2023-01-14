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
    private readonly DbSet<User> _userDbSet;
    private readonly DbSet<Activity> _activityDbSet;
    public AppDbContext _context { get; }

    public CreateUserActivityCommandHandler(AppDbContext context)
    {
        _context = context;
        _userActivityDbSet = context.Set<UserActivity>();
        _userDbSet = context.Set<User>();
        _activityDbSet = context.Set<Activity>();
    }

    public async Task<ErrorOr<UserActivity>> Handle(
        CreateUserActivityCommand request,
        CancellationToken cancellationToken
    )
    {
        var newActivity = request.Adapt<UserActivity>();
        var hasUserAndActivityResult = await HasUserAndActivity(newActivity);

        if (hasUserAndActivityResult.IsError)
        {
            return hasUserAndActivityResult.Errors;
        }

        await _userActivityDbSet.AddAsync(newActivity);
        await _context.SaveChangesAsync();

        return newActivity;
    }

    private async Task<ErrorOr<Success>> HasUserAndActivity(UserActivity userActivity)
    {
        var errors = new List<Error>();

        var userFound = await _userDbSet.AnyAsync(u => u.Id == userActivity.UserId);
        if (!userFound)
        {
            errors.Add(UserError.NotFound);
        }

        var activityFound = await _activityDbSet.AnyAsync(a => a.Id == userActivity.ActivityId);
        if (!activityFound)
        {
            errors.Add(ActivityErrors.NotFound);
        }

        return errors.Any() ? errors : Result.Success;
    }
}
