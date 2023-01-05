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

public class GetUserActivityResponse
{
    public IEnumerable<UserActivity> UserActivities { get; init; }
}
