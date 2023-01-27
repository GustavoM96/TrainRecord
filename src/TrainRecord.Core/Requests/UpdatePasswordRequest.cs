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

public class UpdatePasswordRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string NewPassword { get; init; }
}
