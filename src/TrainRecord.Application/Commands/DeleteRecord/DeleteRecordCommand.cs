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

namespace TrainRecord.Application.DeleteRecord;

public class DeleteRecordCommand : IRequest<ErrorOr<Deleted>>
{
    public Guid RecordId { get; init; }
}

public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand, ErrorOr<Deleted>>
{
    private readonly IUserActivityRepository _userActivityRepository;

    public DeleteRecordCommandHandler(IUserActivityRepository userActivityRepository)
    {
        _userActivityRepository = userActivityRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteRecordCommand request,
        CancellationToken cancellationToken
    )
    {
        var deleted = await _userActivityRepository.DeleteIfExistsById(request.RecordId);
        if (!deleted)
        {
            return UserActivityErrors.NotFound;
        }

        return Result.Deleted;
    }
}
