using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.CreateActivity;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Application.CreateActivity;

public class CreateActivityCommand : IRequest<ErrorOr<Activity>>
{
    public string Name { get; init; }
}

public class CreateActivityCommandHandler
    : IRequestHandler<CreateActivityCommand, ErrorOr<Activity>>
{
    private readonly DbSet<Activity> _activityDbSet;
    public AppDbContext _context { get; }

    public CreateActivityCommandHandler(AppDbContext context)
    {
        _context = context;
        _activityDbSet = context.Set<Activity>();
    }

    public async Task<ErrorOr<Activity>> Handle(
        CreateActivityCommand request,
        CancellationToken cancellationToken
    )
    {
        var newActivity = request.Adapt<Activity>();

        var userFound = await _activityDbSet.AnyAsync(a => a.Name == request.Name);
        if (userFound)
        {
            return ActivityErrors.NameExists;
        }

        await _activityDbSet.AddAsync(newActivity);
        await _context.SaveChangesAsync();

        return newActivity;
    }
}
