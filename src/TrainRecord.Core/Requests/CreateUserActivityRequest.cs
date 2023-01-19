using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Requests;

public class CreateUserActivityRequest
{
    public int Weight { get; init; }
    public int Repetition { get; init; }
}
