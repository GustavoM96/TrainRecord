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
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Application.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    private readonly string _passwordPattern =
        @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,100})";

    public LoginUserCommandValidator()
    {
        RuleFor(u => u.Email).EmailAddress().NotEmpty();
        RuleFor(u => u.Password).Matches(_passwordPattern).NotEmpty();
    }
}
