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
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Application.CreateActivity;

public class CreateActivityCommand : IRequest<ErrorOr<Activity>>
{
    public string Name { get; init; }
}

public class CreateActivityCommandHandler
    : IRequestHandler<CreateActivityCommand, ErrorOr<Activity>>
{
    private readonly IActivityRepository _activityRepository;

    public CreateActivityCommandHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<ErrorOr<Activity>> Handle(
        CreateActivityCommand request,
        CancellationToken cancellationToken
    )
    {
        var newActivity = request.Adapt<Activity>();

        var anyActivity = await _activityRepository.AnyByNameAsync(request.Name);
        if (anyActivity)
        {
            return ActivityErrors.NameExists;
        }

        await _activityRepository.AddAsync(newActivity);
        return newActivity;
    }
}
