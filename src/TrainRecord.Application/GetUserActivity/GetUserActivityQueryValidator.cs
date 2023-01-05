using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.GetUserActivity;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Application.GetUserActivity;

public class GetUserActivityQueryValidator : AbstractValidator<GetUserActivityQuery>
{
    public GetUserActivityQueryValidator() { }
}
