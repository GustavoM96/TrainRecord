﻿using System.Net;
using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Base;
using TrainRecord.Application.CreateActivity;
using TrainRecord.Application.CreateUserActivity;
using TrainRecord.Application.Errors;
using TrainRecord.Application.GetAllActivity;
using TrainRecord.Application.GetUserActivity;
using TrainRecord.Application.LoginUser;
using TrainRecord.Application.RegisterUser;
using TrainRecord.Core.Enum;

namespace TrainRecord.Controllers;

[ApiController]
public class ActivityController : ApiController
{
    [HttpPost]
    [Authorize(Roles = "Adm")]
    public async Task<IActionResult> Create(CreateActivityCommand createActivityCommand)
    {
        var registerResult = await Mediator.Send(createActivityCommand);

        return registerResult.Match<IActionResult>(
            result => CreatedAtAction("GetActivitiesByUser", result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] GetAllActivityQuery getAllActivityQuery)
    {
        var registerResult = await Mediator.Send(getAllActivityQuery);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }
}
