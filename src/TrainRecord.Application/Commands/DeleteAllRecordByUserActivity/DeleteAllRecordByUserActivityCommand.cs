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

namespace TrainRecord.Application.DeleteAllRecordByUserActivity;

public class DeleteAllRecordByUserActivityCommand : IRequest<ErrorOr<Deleted>>
{
    public Guid UserId { get; init; }
    public Guid ActivityId { get; init; }
}

public class DeleteAllRecordByUserActivityCommandHandler
    : IRequestHandler<DeleteAllRecordByUserActivityCommand, ErrorOr<Deleted>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public DeleteAllRecordByUserActivityCommandHandler(
        IUserActivityRepository userActivityRepository
    )
    {
        _userActivityRepository = userActivityRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteAllRecordByUserActivityCommand request,
        CancellationToken cancellationToken
    )
    {
        var records = _userActivityRepository.GetAllRecordByUserAndActivityId(
            request.UserId,
            request.ActivityId
        );

        if (!records.Any())
        {
            return UserActivityErrors.NotFound;
        }

        _userActivityRepository.DeleteAll(records);
        return Result.Deleted;
    }
}
