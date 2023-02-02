using System.Net;
using System.Security.Claims;
using ErrorOr;
using LaDeak.JsonMergePatch.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.CreateActivity;
using TrainRecord.Application.CreateUserActivity;
using TrainRecord.Application.DeleteAllRecordByUserActivity;
using TrainRecord.Application.DeleteRecord;
using TrainRecord.Application.Errors;
using TrainRecord.Application.GetActivityByUserQuery;
using TrainRecord.Application.GetAllUserQuery;
using TrainRecord.Application.GetRecordQuery;
using TrainRecord.Application.GetUserByIdQuery;
using TrainRecord.Application.UpdateUser;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Requests;

namespace TrainRecord.Controllers;

[ApiController]
public class TeacherController : ApiController
{
    [HttpGet("[action]")]
    [Authorize(Policy = "IsStudent")]
    public async Task<IActionResult> RegisterToStudent(
        Guid userId,
        Guid activityId,
        [FromQuery] Pagination pagination
    )
    {
        var query = new GetRecordQuery()
        {
            UserId = userId,
            ActivityId = activityId,
            Pagination = pagination
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
    {
        var query = new GetAllUserQuery() { Pagination = pagination, Role = Role.Teacher };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }
}
