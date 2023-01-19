using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.CreateUserActivity;

public class CreateUserActivityCommandValidator : AbstractValidator<CreateUserActivityCommand>
{
    public CreateUserActivityCommandValidator()
    {
        RuleFor(ua => ua.Repetition).GreaterThan(0);
        RuleFor(ua => ua.Weight).GreaterThanOrEqualTo(0);
    }
}
